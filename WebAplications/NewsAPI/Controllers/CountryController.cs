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
    public class CountryController : ControllerBase
    {
        public readonly NewsDbContext _newsDbContext;

        public CountryController(NewsDbContext newsDbContext)
        {
            _newsDbContext = newsDbContext;
        }

        [HttpGet]
        [Route("AllCountries")]
        public IActionResult GetAllCountries()
        {

            try
            {
                var Countries = _newsDbContext.Countries.ToList();
                return Ok(Countries);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("AllCountries/{id:int}")]
        public IActionResult GetById(int id)
        {

            Country CountriesId = _newsDbContext.Countries.Find(id);
            if (CountriesId == null)
            {

                return BadRequest("Categoria No encontrado");

            }

            try
            {
                CountriesId = _newsDbContext.Countries.
                    Where(a => a.CountryId == id).FirstOrDefault();

                return Ok(CountriesId);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody] Country Object)
        {
            try
            {
                _newsDbContext.Countries.Add(Object);
                _newsDbContext.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditCountry([FromBody] Country Object)
        {

            Country countriesobject = _newsDbContext.Countries.Find(Object.CountryId);
            if (countriesobject == null)
            {

                return BadRequest("Categoria No encontrado");

            }

            try
            {
                countriesobject.Name = Object.Name is null ? countriesobject.Name : Object.Name;
                _newsDbContext.Countries.Update(countriesobject);
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
        public IActionResult DeleteCountry(int id)
        {

            Country DeleteCountries = _newsDbContext.Countries.Find(id);

            if (DeleteCountries == null)
            {

                return BadRequest("Country No encontrado");

            }

            try
            {

                _newsDbContext.Countries.Remove(DeleteCountries);
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
