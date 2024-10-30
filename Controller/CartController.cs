using Microsoft.AspNetCore.Mvc;
using MyShopAPI.Models;
using MyShopAPI.Services;
using System.Threading.Tasks;

namespace MyShopAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        // Adicionar produto ao carrinho
        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddProductToCart(string productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Produto não encontrado");
            }

            await _cartService.AddProductToCartAsync(product);
            return Ok(await _cartService.GetCartAsync());
        }

        // Remover produto do carrinho
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(string productId)
        {
            await _cartService.RemoveProductFromCartAsync(productId);
            return Ok(await _cartService.GetCartAsync());
        }

        // Obter o carrinho completo
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCartAsync();
            return Ok(cart);
        }

        // Limpar o carrinho
        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return Ok();
        }
    }
}
