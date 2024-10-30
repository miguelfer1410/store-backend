using MongoDB.Bson;
using MongoDB.Driver;
using MyShopAPI.Models;
using MyShopAPI.Data;

namespace MyShopAPI.Services
{
    public class FavoriteService
    {
        private readonly IMongoCollection<Favorite> _favorites;
        private readonly IMongoCollection<Product> _products;
        private readonly MongoDbContext _context;

        public FavoriteService(MongoDbContext context)
        {
            _context = context;
            _favorites = context.Favorites;
            _products = context.Products;
        }

        public async Task<List<Product>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _favorites
                .Find(f => f.UserId == userId)
                .ToListAsync();

            var productIds = favorites.Select(f => f.ProductId).ToList();
            
            // Buscar os produtos correspondentes
            var products = await _products
                .Find(p => productIds.Contains(p.Id))
                .ToListAsync();

            return products;
        }

        public async Task AddToFavoritesAsync(string userId, string productId)
        {
            var existingFavorite = await _favorites
                .Find(f => f.UserId == userId && f.ProductId == productId)
                .FirstOrDefaultAsync();

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                };
                await _favorites.InsertOneAsync(favorite);
            }
        }

        public async Task RemoveFromFavoritesAsync(string userId, string productId)
        {
            await _favorites.DeleteOneAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        public async Task<bool> IsFavoriteAsync(string userId, string productId)
        {
            var favorite = await _favorites
                .Find(f => f.UserId == userId && f.ProductId == productId)
                .FirstOrDefaultAsync();
            return favorite != null;
        }
    }
}