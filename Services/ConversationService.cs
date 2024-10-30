using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyShopAPI.Models;
using MyShopAPI.Data;

namespace MyShopAPI.Services
{
    public class ConversationService
    {
        private readonly IMongoCollection<Conversation> _conversations;

        public ConversationService(MongoDbContext context)
        {
            _conversations = context.Database.GetCollection<Conversation>("Conversations");
        }

        // Método para obter todas as conversas
        public async Task<List<Conversation>> GetAllConversationsAsync()
        {
            return await _conversations.Find(_ => true).SortBy(c => c.CreatedAt).ToListAsync();
        }

        // Método para deletar uma conversa específica
        public async Task<DeleteResult> DeleteConversationAsync(string id)
        {
            var filter = Builders<Conversation>.Filter.Eq(c => c.Id, id);
            return await _conversations.DeleteOneAsync(filter);
        }

        // Método para buscar todas as mensagens de uma conversa específica
        public async Task<List<Message>> GetMessagesByConversationIdAsync(string conversationId)
        {
            var filter = Builders<Conversation>.Filter.Eq(c => c.Id, conversationId);
            var conversation = await _conversations.Find(filter).FirstOrDefaultAsync();

            return conversation?.Messages;
        }

        // Método para iniciar uma nova conversa com uma mensagem inicial
        // Método para iniciar uma nova conversa com uma mensagem inicial
        public async Task<Conversation> StartConversationAsync(string content, string userId, string firstName, string lastName, Product product)
            {
                var conversation = new Conversation
                {
                    UserId = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    Product = product,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Content = content,
                            UserId = userId,
                            Timestamp = DateTime.UtcNow
                        }
                    }
                };

                await _conversations.InsertOneAsync(conversation);
                return conversation;
            }

        // Novo método para adicionar uma mensagem a uma conversa
        public async Task<List<Message>> AddMessageToConversationAsync(string conversationId, string content, bool isFromAdmin)
        {
            var filter = Builders<Conversation>.Filter.Eq(c => c.Id, conversationId);
            var conversation = await _conversations.Find(filter).FirstOrDefaultAsync();

            if (conversation == null)
            {
                return null;
            }

            var newMessage = new Message
            {
                Content = content,
                IsFromAdmin = isFromAdmin,
                UserId = isFromAdmin ? "admin" : conversation.UserId,
                Timestamp = DateTime.UtcNow
            };

            conversation.Messages.Add(newMessage);
            conversation.UpdatedAt = DateTime.UtcNow;

            await _conversations.ReplaceOneAsync(filter, conversation);

            return conversation.Messages;
        }

        // Método para obter todas as conversas de um usuário específico
        public async Task<List<Conversation>> GetConversationsByUserIdAsync(string userId)
        {
            var filter = Builders<Conversation>.Filter.And(
                Builders<Conversation>.Filter.Eq(c => c.UserId, userId),
                Builders<Conversation>.Filter.Not(Builders<Conversation>.Filter.AnyEq(c => c.DeletedForUsers, userId))
            );
            return await _conversations.Find(filter)
                                   .SortByDescending(c => c.UpdatedAt)
                                   .ToListAsync();
        }

        public async Task<bool> MarkConversationAsDeletedForUserAsync(string conversationId, string userId)
        {
            var filter = Builders<Conversation>.Filter.Eq(c => c.Id, conversationId);
            var update = Builders<Conversation>.Update.AddToSet(c => c.DeletedForUsers, userId);

            var result = await _conversations.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
