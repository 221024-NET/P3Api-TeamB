using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /*** old ADO stuff Kept in comments for reference ***/

        //private readonly IRepository _repo;
        private readonly ILogger<ProductController> _logger;

        //public ProductController(IRepository repo, ILogger<ProductController> logger)
        //{
        //    this._repo = repo;
        //    this._logger = logger;
        //}

        private readonly IContext _context;

        public ProductController(IContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetOne(int id)
        {
            _logger.LogInformation("api/product/{id} triggered");
            //return new Product(0, "food item", 5, 36, "tastes good, is good", "");

            var item = await _context.GetProductById(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;


            //try
            //{
            //    return Ok(await _repo.GetProductByIdAsync(id));
            //    _logger.LogInformation("api/product/{id} completed successfully");
            //}
            //catch
            //{
            //    return BadRequest();
            //    _logger.LogWarning("api/product/{id} completed with errors");
            //}
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            _logger.LogInformation("api/product triggered");


            IEnumerable<Product> products = await _context.GetAllProducts();

            if (products == null)
            {
                return NotFound();
            }

            return products.ToList();

            //try
            //{
            //    return Ok(await _repo.GetAllProductsAsync());
            //    _logger.LogInformation("api/product completed successfully");
            //}
            //catch
            //{
            //    return BadRequest();
            //    _logger.LogWarning("api/product completed with errors");
            //}
        }

        [HttpPatch]
        public async Task<ActionResult<Product[]>> Purchase([FromBody] Product[] purchaseProducts)
        {
            _logger.LogInformation("PATCH api/product triggered");
            List<Product> products = new List<Product>();
            try
            {
                foreach (Product item in purchaseProducts)
                {
                    var tmp = await _context.GetProductById(item.ProductId);
                    //Product tmp = await _repo.GetProductByIdAsync(item.id);
                    int quantityDiff = tmp.ProductQuantity - item.ProductQuantity;
                    if (quantityDiff >= 0)
                    {
                        item.ProductQuantity = quantityDiff;
                        _context.UpdateProduct(item);

                        //await _repo.reduceinventorybyidasync(item.id, item.quantity);
                        var updatedTmp = await _context.GetProductById(item.ProductId);
                        Product prod = updatedTmp;
                        products.Add(prod);
                    }
                    else
                    {
                        throw new Exception("Insuffecient inventory.");
                    }
                }
                return Ok(products);
                _logger.LogInformation("PATCH api/product completed successfully");
            }
            catch
            {
                return BadRequest();
                _logger.LogWarning("PATCH api/product completed with errors");

            }


        }

    }
}
