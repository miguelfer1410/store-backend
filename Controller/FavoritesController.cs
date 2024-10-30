using Microsoft.AspNetCore.Mvc;
using MyShopAPI.Models;
using MyShopAPI.Services;

namespace MyShopAPI.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;

        public FavoritesController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Product>>> GetUserFavorites(string userId)
        {
            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost("{userId}/{productId}")]
        public async Task<IActionResult> AddToFavorites(string userId, string productId)
        {
            await _favoriteService.AddToFavoritesAsync(userId, productId);
            return Ok();
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(string userId, string productId)
        {
            await _favoriteService.RemoveFromFavoritesAsync(userId, productId);
            return Ok();
        }

        [HttpGet("{userId}/{productId}")]
        public async Task<ActionResult<bool>> IsFavorite(string userId, string productId)
        {
            var isFavorite = await _favoriteService.IsFavoriteAsync(userId, productId);
            return Ok(isFavorite);
        }
    }
} 