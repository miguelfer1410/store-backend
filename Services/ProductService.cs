using MongoDB.Driver;
using MyShopAPI.Models;
using MyShopAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShopAPI.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(MongoDbContext context)
        {
            _products = context.Products;
        }

        // Obtém todos os produtos
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _products.Find(product => true).ToListAsync();
        }

        // Obtém um produto por ID
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products.Find(product => product.Id == id).FirstOrDefaultAsync();
        }

        // Adiciona um novo produto, incluindo imagens
        public async Task AddProductAsync(Product product, List<string> imageUrls)
        {
            // Adicionar os URLs das imagens ao produto
            product.ImageUrls = imageUrls;

            // Inserir o produto no MongoDB
            await _products.InsertOneAsync(product);
        }

        // Atualiza um produto existente
        public async Task UpdateProductAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == id, product);
        }

        // Deleta um produto
        public async Task DeleteProductAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }
    }
}
