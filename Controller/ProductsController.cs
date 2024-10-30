using Microsoft.AspNetCore.Mvc;
using MyShopAPI.Models;
using MyShopAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShopAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            // Ajustar o caminho das imagens
            foreach (var product in products)
            {
                if (product.ImageUrls != null)
                {
                    product.ImageUrls = product.ImageUrls.Select(url => url.Replace("\\", "/")).ToList();
                }
            }

            return products;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(
            [FromForm] string name, 
            [FromForm] string description, 
            [FromForm] decimal price, 
            [FromForm] List<IFormFile> images,
            [FromForm] string category, 
            [FromForm] string status,
            [FromForm] string? brand = null,
            [FromForm] string? clothingType = null,
            [FromForm] string? color = null) // Novo parâmetro
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                Status = status,
                Brand = brand,
                ClothingType = clothingType,
                Color = color // Novo campo
            };

            var imageUrls = new List<string>();

            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var uploadsFolder = Path.Combine("Uploads"); // Diretório onde as imagens serão salvas
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder); // Cria o diretório se não existir
                    }

                    var filePath = Path.Combine(uploadsFolder, image.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Adiciona o caminho da imagem à lista
                    imageUrls.Add(filePath);
                }
            }

            // Chama o serviço para adicionar o produto junto com as URLs das imagens
            await _productService.AddProductAsync(product, imageUrls);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
