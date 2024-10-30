using MongoDB.Driver;
using MyShopAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartService
{
    private readonly IMongoCollection<Cart> _cartCollection;

    public CartService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MyShopDB");
        _cartCollection = database.GetCollection<Cart>("Carts");
    }

    // Obter o carrinho
    public async Task<Cart> GetCartAsync()
    {
        return await _cartCollection.Find(cart => true).FirstOrDefaultAsync() ?? await CreateCartAsync();
    }

    // Criar um novo carrinho
    public async Task<Cart> CreateCartAsync()
    {
        var cart = new Cart();
        await _cartCollection.InsertOneAsync(cart);
        return cart;
    }

    // Adicionar um produto ao carrinho
    public async Task AddProductToCartAsync(Product product)
    {
        var cart = await GetCartAsync();
        var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == product.Id);
        
        if (existingItem == null)
        {
            cart.Items.Add(new CartItem { Product = product });
            await _cartCollection.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        }
    }

    // Remover um produto do carrinho
    public async Task RemoveProductFromCartAsync(string productId)
    {
        var cart = await GetCartAsync();
        if (cart != null)
        {
            cart.Items = cart.Items.Where(i => i.Product.Id != productId).ToList();
            await _cartCollection.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        }
    }

    // Limpar o carrinho
    public async Task ClearCartAsync()
    {
        var cart = await GetCartAsync();
        if (cart != null)
        {
            cart.Items.Clear();
            await _cartCollection.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        }
    }
}
