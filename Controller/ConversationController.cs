using Microsoft.AspNetCore.Mvc;
using MyShopAPI.Models;
using MyShopAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShopAPI.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly ConversationService _conversationService;

        public ConversationsController(ConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        // Rota para buscar todas as conversas
        [HttpGet]
        public async Task<ActionResult<List<Conversation>>> GetAllConversations()
        {
            var conversations = await _conversationService.GetAllConversationsAsync();
            
            return Ok(conversations);
        }

        // Rota para deletar uma conversa específica
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            var result = await _conversationService.DeleteConversationAsync(id);
            if (result.DeletedCount == 0)
            {
                return NotFound("Conversa não encontrada.");
            }
            return NoContent();
        }

        // Rota para buscar todas as mensagens de uma conversa específica
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<List<Message>>> GetMessagesByConversationId(string id)
        {
            var messages = await _conversationService.GetMessagesByConversationIdAsync(id);
            if (messages == null)
            {
                return NotFound("Conversa não encontrada.");
            }
            return Ok(messages);
        }

        // Rota para criar uma nova conversa ao postar uma mensagem inicial
        [HttpPost("start")]
    public async Task<IActionResult> StartConversation([FromBody] ConversationStartRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.UserId))
        {
            return BadRequest("Conteúdo da mensagem, UserId, FirstName e LastName são obrigatórios.");
        }

        // Chama o serviço para criar a conversa e adicionar a mensagem
        var conversation = await _conversationService.StartConversationAsync(
            request.Content,
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Product
        );

        return CreatedAtAction(nameof(GetMessagesByConversationId), new { id = conversation.Id }, conversation);
    }

        // Novo endpoint para adicionar uma mensagem a uma conversa
        [HttpPost("{id}/messages")]
        public async Task<ActionResult<List<Message>>> AddMessageToConversation(string id, [FromBody] MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("O conteúdo da mensagem não pode estar vazio.");
            }

            var updatedMessages = await _conversationService.AddMessageToConversationAsync(id, request.Content, request.IsFromAdmin);
            if (updatedMessages == null)
            {
                return NotFound("Conversa não encontrada.");
            }

            return Ok(updatedMessages);
        }

        // Rota para buscar todas as conversas de um usuário específico
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Conversation>>> GetConversationsByUserId(string userId)
        {
            var conversations = await _conversationService.GetConversationsByUserIdAsync(userId);
            
            if (conversations == null || !conversations.Any())
            {
                return NotFound("Nenhuma conversa encontrada para este usuário.");
            }

            return Ok(conversations);
        }

// Classe de requisição para iniciar conversa (ajustada)
    public class ConversationStartRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Content { get; set; } // Conteúdo da primeira mensagem
        public Product Product { get; set; }
    }

    public class MessageRequest
    {
        public string Content { get; set; }
        public bool IsFromAdmin { get; set; }
    }
}

}
