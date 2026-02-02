using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using React_dotnet.database;
using React_dotnet.database.Models;
using React_dotnet.Server.Dtos;
using System.Linq;

namespace React_dotnet.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CoreDbContext coreDbContext;

        public ProductController(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        // Mapping function

        Func<Product, ProductDto> mapProductToDto = p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
        };


        // GET /product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            // nagyon vigyázni, mert a tolist memóriába tölti az egészet, célszerű szűrni és lapozni
            var products = await coreDbContext.Products
                .ToListAsync();

            var mapped = products.Select(mapProductToDto);

            return Ok(mapped);
        }

        // GET /product/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductDto>> GetById(long id)
        {
            var product = await coreDbContext.Products
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var mapped = mapProductToDto(product);

            return Ok(mapped);
        }

        // Mapping function
        Func<ProductDto, Product> mapProductDtotoProduct = p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
        };

        // POST /product
        [HttpPost] 
        [Authorize(Roles = BuiltInRoles.Admin)] //csak adminnak
        public async Task<ActionResult<Product>> Post(ProductDto product)
        {
            var mapped = mapProductDtotoProduct(product);

            await coreDbContext.Products.AddAsync(mapped);
            await coreDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }


        //DELETE /product/1
        [HttpDelete("{id:long}")]
        [Authorize(Roles = BuiltInRoles.Admin)] //csak adminnak
        public async Task<ActionResult> Delete (long id) 
        {
            var product = await coreDbContext.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null) 
            {
                return NotFound();
            }

            coreDbContext.Products.Remove(product);
            await coreDbContext.SaveChangesAsync();

            return NoContent();
        }

        // PUT /product/1
        [HttpPut]
        [Authorize(Roles = BuiltInRoles.Admin)] //csak adminnak
        public async Task<ActionResult> Put(ProductDto product)
        {
            
            var existingProduct = await coreDbContext.Products.SingleOrDefaultAsync(p => p.Id == product.Id);

            if(existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            await coreDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
