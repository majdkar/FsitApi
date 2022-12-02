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
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _dbContext;


        public CategoryController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [Produces("application/json")]
        [HttpGet("GetListCategory")]
        public IActionResult GetListCategory()
        {
            var cate = from Category in _dbContext.TblCategory
                       select new
                       {
                           Category.Id,
                           Category.Title,
                           Category.ImageUrlCategory,
                       };
            return Ok(cate);
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("GetProductForCategoryOne")]
        public IActionResult GetProductForCategoryOne([FromQuery]int idcategory)
        {
            var cate = 
                       from Product in _dbContext.TblProduct
                       join Category in _dbContext.TblCategory
                      on Product.IdCategory equals Category.Id
                       where Category.Id == idcategory
                       select new
                       {
                           Product.Id,
                           Product.TitleProduct,
                           Product.Price,
                           Product.Description,
                           Product.CreateOfDate,
                           Category.Title,
                           Product.ImageUrlProduct,
                       };
            return Ok(cate);
        }


        [Produces("application/json")]
        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory([FromQuery]int idcategory)
        {
            var delcategory = _dbContext.TblCategory.Where(c => c.Id == idcategory).First();
            _dbContext.TblCategory.Remove(delcategory);
            _dbContext.SaveChanges();
            return Ok(delcategory);

        }


        [Produces("application/json")]
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategory([FromQuery]string title)
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
                var category = new Category()
                {
                    Title = title,
                    ImageUrlCategory = dbPath
                };
              
                //save the filePath to database ImagePath field.
                _dbContext.TblCategory.Add(category);
                _dbContext.SaveChanges();
                return Ok(category);
                
            }
            else
            {
                return BadRequest();
            }

        }


        [Produces("application/json")]
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult> UpdateCategory([FromForm]Category cate)
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
                var categories = _dbContext.TblCategory.Where(p => p.Id == cate.Id).First();
                categories.Title = cate.Title;
                categories.ImageUrlCategory = dbPath;
                _dbContext.SaveChanges();
                return Ok(categories);

            }
            else
            {
                return BadRequest();
            }
        }

    }
}