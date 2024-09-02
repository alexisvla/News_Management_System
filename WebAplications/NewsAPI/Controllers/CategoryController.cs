using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAPI.Data;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    [EnableCors("ReglaCors")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly NewsDbContext _newsDbContext;

        public CategoryController(NewsDbContext newsDbContext)
        {
            _newsDbContext = newsDbContext;
        }

        [HttpGet]
        [Route("AllCategories")]
        public IActionResult GetAllCategory()
        {

            try
            {
                var categories = _newsDbContext.Categories.ToList();
                return Ok(categories);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("AllCategories/{id:int}")]
        public IActionResult GetById(int id)
        {

            Category categoriesId = _newsDbContext.Categories.Find(id);
            if (categoriesId == null)
            {

                return BadRequest("Categoria No encontrado");

            }

            try
            {
                categoriesId = _newsDbContext.Categories.
                    Where(a => a.CategoryId == id).FirstOrDefault();

                return Ok(categoriesId);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category Object)
        {
            try
            {
                _newsDbContext.Categories.Add(Object);
                _newsDbContext.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditCategory([FromBody] Category Object)
        {

            Category categoriesobject = _newsDbContext.Categories.Find(Object.CategoryId);
            if (categoriesobject == null)
            {

                return BadRequest("Categoria No encontrado");

            }

            try
            {
                categoriesobject.Name = Object.Name is null ? categoriesobject.Name : Object.Name;
                _newsDbContext.Categories.Update(categoriesobject);
                _newsDbContext.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public IActionResult DeleteCategory(int id)
        {

            Category Deletenews = _newsDbContext.Categories.Find(id);

            if (Deletenews == null)
            {

                return BadRequest("Categoria No encontrado");

            }

            try
            {

                _newsDbContext.Categories.Remove(Deletenews);
                _newsDbContext.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
