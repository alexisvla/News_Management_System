using Microsoft.AspNetCore.Mvc;
using NEWS_WebAplication.Models;
using NewsAPI.Data;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace NEWS_WebAplication.Controllers
{
    public class CountryController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5116/api");
        private readonly HttpClient _httpClient;
        private readonly NewsDbContext _newsDbContext;

        public CountryController(HttpClient httpClient, NewsDbContext newsDbContext)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
            _newsDbContext = newsDbContext;
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AllCountries()
        {
            List<CountryViewModel> CountryList = new List<CountryViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Country/GetAllCountries/AllCountries").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                CountryList = JsonConvert.DeserializeObject<List<CountryViewModel>>(data);
            }

            return View(CountryList);
        }


        public IActionResult CreateCountries()
        {

            return View(new CountryViewModel());
        }

        [HttpPost]
        public IActionResult CreateCountries(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
				var countries = new Country();
				countries.Name = model.Name;
				_newsDbContext.Add(countries);
				_newsDbContext.SaveChanges();
				
				return RedirectToAction("AllCountries");
			}
            return View(model);

		}


        public IActionResult EditCountries(int id)
        {
            var data = _newsDbContext.Countries.FirstOrDefault(x => x.CountryId == id);

            if (data == null)
            {
                return NotFound();
            }

            var model = new CountryViewModel
            {
                Name = data.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCountries(int id, CountryViewModel model)
        {

            var data = _newsDbContext.Countries.FirstOrDefault(x => x.CountryId == id);
            if (data != null && ModelState.IsValid)
            {
                data.Name = model.Name;
                _newsDbContext.Countries.Update(data);
                _newsDbContext.SaveChanges();

                return RedirectToAction("AllCountries");
            }
            else
            {
                return View(model);
            }


        }


        public IActionResult DeleteCountries(int id)
        {

            var data = _newsDbContext.Countries.FirstOrDefault(x => x.CountryId == id);
            if (data == null)
            {
                return NotFound();
            }



            var model = new CountryViewModel
            {
                Name = data.Name
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult DeleteCountries(int id, CountryViewModel model)
        {

            var data = _newsDbContext.Countries.FirstOrDefault(x => x.CountryId == id);
            if (data != null && ModelState.IsValid)
            {
                data.Name = model.Name;
                _newsDbContext.Countries.Remove(data);
                _newsDbContext.SaveChanges();

				return RedirectToAction("AllCountries"); 
            }
            else
            {
                return View(model);
            }



        }
    }
}
