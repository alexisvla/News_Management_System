using Microsoft.AspNetCore.Mvc;
using NEWS_WebAplication.Models;
using NewsAPI.Data;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace NEWS_WebAplication.Controllers
{
    public class CategoryController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5116/api");
        private readonly HttpClient _httpClient;
        private readonly NewsDbContext _newsDbContext;

        public CategoryController(HttpClient httpClient, NewsDbContext newsDbContext)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
            _newsDbContext = newsDbContext;
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AllCategories()
        {
            List<CategoryViewModel> CategoryList = new List<CategoryViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Category/GetAllCategory/AllCategories").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                CategoryList = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);
            }

            return View(CategoryList);
        }


        public IActionResult CreateCategories()
        {

            return View(new CategoryViewModel());
        }

        [HttpPost]
        public IActionResult CreateCategories(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
				var categories = new Category();
				categories.Name = model.Name;
				_newsDbContext.Add(categories);
				_newsDbContext.SaveChanges();
				
				return RedirectToAction("AllCategories");
			}
            return View(model);

		}


        public IActionResult EditCategories(int id)
        {
            var data = _newsDbContext.Categories.FirstOrDefault(x => x.CategoryId == id);

            if (data == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                Name = data.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategories(int id, CategoryViewModel model)
        {

            var data = _newsDbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (data != null && ModelState.IsValid)
            {
                data.Name = model.Name;
                _newsDbContext.Categories.Update(data);
                _newsDbContext.SaveChanges();

                return RedirectToAction("AllCategories");
            }
            else
            {
                return View(model);
            }


        }


        public IActionResult DeleteCategories(int id)
        {

            var data = _newsDbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (data == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                Name = data.Name
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult DeleteCategories(int id, CategoryViewModel model)
        {

            var data = _newsDbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (data != null && ModelState.IsValid )
            {
                data.Name = model.Name;
                _newsDbContext.Categories.Remove(data);
                _newsDbContext.SaveChanges();

				return RedirectToAction("AllCategories"); 
            }
            else
            {
                return View(model);
            }



        }
    }
}
