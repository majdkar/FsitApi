using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskEmployees.Model;

namespace TaskEmployees.Controllers
{
    [Authorize]
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ProductController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [Produces("application/json")]
        [HttpGet("GetListProduct")]
        public IActionResult GetListProduct()
        {
            var product = from Product in _dbContext.TblProduct
                       join Category in _dbContext.TblCategory
                       on Product.IdCategory equals Category.Id
                       select new
                       {
                           Product.Id,
                           Product.TitleProduct,
                           Product.Description,
                           Category.Title,
                           Product.Price,
                           Product.CreateOfDate,
                           Product.ImageUrlProduct,
                       };
            return Ok(product);
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("GetProductOne")]
        public IActionResult GetProductOne([FromQuery]int idproduct)
        {
            var product = from Product in _dbContext.TblProduct
                       join Category in _dbContext.TblCategory
                      on Product.IdCategory equals Category.Id
                       where Product.Id == idproduct
                       select new
                       {
                           Product.Id,
                           Product.TitleProduct,
                           Product.Description,
                           Category.Title,
                           Product.Price,
                           Product.CreateOfDate,
                           Product.ImageUrlProduct,
                       };
            return Ok(product);
        }


        [Produces("application/json")]
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct([FromQuery]int idproduct)
        {
            var delproduct = _dbContext.TblProduct.Where(p => p.Id == idproduct).First();
            _dbContext.TblProduct.Remove(delproduct);
            _dbContext.SaveChanges();
            return Ok(delproduct);

        }


        [Produces("application/json")]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromForm]Product prod)
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var product = new Product()
                {
                    TitleProduct = prod.TitleProduct,
                    Description = prod.Description,
                    Price = prod.Price,
                    CreateOfDate = prod.CreateOfDate,
                    IdCategory = prod.IdCategory,
                    ImageUrlProduct = dbPath
                };

                //save the filePath to database ImagePath field.
                _dbContext.TblProduct.Add(product);
                _dbContext.SaveChanges();
                return Ok(product);

            }
            else
            {
                return BadRequest();
            }

        }


        [Produces("application/json")]
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct([FromForm]Product prod)
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var product = _dbContext.TblProduct.Where(p => p.Id == prod.Id).First();
                product.TitleProduct = prod.TitleProduct;
                product.Description = prod.Description;
                product.Price = prod.Price;
                product.CreateOfDate = prod.CreateOfDate;
                product.IdCategory = prod.IdCategory;
                product.ImageUrlProduct = dbPath;
                _dbContext.SaveChanges();
                return Ok(product);
            }
            else
            {
                return BadRequest();
            }
        }


    }
}