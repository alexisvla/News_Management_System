using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewsAPI.Data;
using NewsAPI.Models;
using System.Security.Claims;

namespace NewsAPI.Controllers
{
    [EnableCors("ReglaCors")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        public readonly NewsDbContext _newsDbContext;

        public NewsController(NewsDbContext newsDbContext)
        {
            _newsDbContext = newsDbContext;
        }

        [HttpGet]
        [Route("AllNews")]
        public IActionResult GetAllNews()
        {
            
            try
            {
               var news = _newsDbContext.News.Include(a => a.Category).Include(b => b.Country).ToList();
                return Ok(news);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllNews")]
        public ActionResult<IEnumerable<News>> GetAllNews([FromQuery] string category = null, [FromQuery] string country = null)
        {
            try
            {
                IQueryable<News> query = _newsDbContext.News.Include(a => a.Category).Include(b => b.Country);

                // Aplicar filtros según los parámetros recibidos
                if (!string.IsNullOrEmpty(category))
                {
                    // Aplicar filtro por categoría si el parámetro no está vacío
                    query = query.Where(a => a.Category.Name == category);
                }
                else if (!string.IsNullOrEmpty(country))
                {
                    // Aplicar filtro por país si el parámetro de categoría está vacío y el de país no
                    query = query.Where(a => a.Country.Name == country);
                }

                List<News> newsList = query.ToList();

                if (newsList.Count == 0)
                {
                    return NotFound("Noticias no encontradas");
                }

                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]

        public IActionResult CreateNew([FromBody] News Object)
        {
            try
            {
                _newsDbContext.News.Add(Object);
                _newsDbContext.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
		public IActionResult EditNew([FromBody] News Object)
		{

            News newsobject = _newsDbContext.News.Find(Object.NewsId);
            if (newsobject == null)
            {

                return BadRequest("Producto No encontrado");

            }

			try
			{
				newsobject.Author = Object.Author is null ? newsobject.Author : Object.Author;
                newsobject.Title = Object.Title is null ? newsobject.Title : Object.Title;
                newsobject.Description = Object.Description is null ? newsobject.Description : Object.Description;
                newsobject.Content = Object.Content is null ? newsobject.Content : Object.Content;
                newsobject.ImagePath = Object.ImagePath is null ? newsobject.ImagePath : Object.ImagePath;
                newsobject.PublishedAt = Object.PublishedAt;
                newsobject.CategoryId = Object.CategoryId; 
                newsobject.CountryId = Object.CountryId;
                _newsDbContext.News.Update(newsobject);
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
        public IActionResult DeleteNew(int id)
        {

            News Deletenews = _newsDbContext.News.Find(id);

            if (Deletenews == null)
            {

                return BadRequest("Producto No encontrado");

            }

            try
            {
               
                _newsDbContext.News.Remove(Deletenews);
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
