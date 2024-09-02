using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAPI.Data;
using NewsAPI.Models;

using NEWS_WebAplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace NEWS_WebAplication.Controllers
{
    public class NewsController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5116/api");
        private readonly HttpClient _httpClient;
        private readonly NewsDbContext _newsDbContext;

        public NewsController(HttpClient httpClient, NewsDbContext newsDbContext)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
            _newsDbContext = newsDbContext;
        }


        
        [HttpGet]
        public IActionResult AllNews()
        {
            List<NewsViewModel> NewsList = new List<NewsViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllNews/AllNews").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                NewsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);
            }

            return View(NewsList);
        }

        public IActionResult AllNewsMember()
        {
            List<NewsViewModel> userNewsList = new List<NewsViewModel>();

            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllNews/AllNews").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<NewsViewModel> allNewsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);

                    // Filtrar las noticias creadas por el usuario actual
                    userNewsList = allNewsList.Where(news => news.UserId == userId).ToList();
                }

                return View(userNewsList);
            }
            else
            {
                return Unauthorized(); // Usuario no autenticado o reclamo no encontrado
            }

            
        }



        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CRUDNews()
        {
            List<NewsViewModel> NewsList = new List<NewsViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllNews/AllNews").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                NewsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);
            }

            return View(NewsList);
        }

        [HttpGet]
        public IActionResult CRUDNewsMember()
        {
            List<NewsViewModel> userNewsList = new List<NewsViewModel>();

            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllNews/AllNews").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<NewsViewModel> allNewsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);

                    // Filtrar las noticias creadas por el usuario actual
                    userNewsList = allNewsList.Where(news => news.UserId == userId).ToList();
                }

                return View(userNewsList);
            }
            else
            {
                return Unauthorized(); // Usuario no autenticado o reclamo no encontrado
            }
        }

        [HttpGet]
        public IActionResult WatchNews(int id)
        {
            
            List<NewsViewModel> userNewsList = new List<NewsViewModel>();

            var NewsById = _newsDbContext.News.FirstOrDefault(x => x.NewsId == id); 

            if (NewsById != null)
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllNews/AllNews").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<NewsViewModel> allNewsList = JsonConvert.DeserializeObject<List<NewsViewModel>>(data);

                   
                    userNewsList = allNewsList.Where(news => news.NewsId == NewsById.NewsId).ToList();
                }

                return View(userNewsList);
            }
            else
            {
                return Unauthorized(); 
            }
        }

        //public IActionResult CreateNewsMember()
        //{

        //    ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
        //    ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");
        //    return View(new NewsViewModel());
        //}


        //[HttpPost]
        //public IActionResult CreateNewsMember(NewsViewModel model)
        //{
        //    var userIdClaim = User.FindFirst("UserId");

        //    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        //    {
        //        var news = new News();
        //        news.Author = model.Author;
        //        news.Title = model.Title;
        //        news.Description = model.Description;
        //        news.Content = model.Content;
        //        news.ImagePath = model.ImagePath;
        //        news.PublishedAt = model.PublishedAt;
        //        news.CategoryId = model.CategoryId;
        //        news.CountryId = model.CountryId;
        //        news.UserId = userId;
        //        _newsDbContext.Add(news);
        //        _newsDbContext.SaveChanges();

        //        ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
        //        ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");


        //        return View(model);
        //    }
        //    else
        //    {
        //        return Unauthorized(); // O cualquier otro código de estado que desees devolver
        //    }          

        //}

        public IActionResult CreateNews()
		{
            
			ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
			ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");
			return View(new NewsViewModel());
		}


		[HttpPost]
        public IActionResult CreateNews(NewsViewModel model, IFormFile imageFile)
        {

            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var news = new News();
                news.Author = model.Author;
                news.Title = model.Title;
                news.Description = model.Description;
                news.Content = model.Content;
                news.PublishedAt = model.PublishedAt;
                news.CategoryId = model.CategoryId;
                news.CountryId = model.CountryId;
                news.UserId = model.UserId = userId;

                // Guardar la imagen como bytes en la base de datos
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        news.ImagePath = memoryStream.ToArray();
                    }
                }

                _newsDbContext.Add(news);
                _newsDbContext.SaveChanges();

                ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
                ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AllNews");
                }

                if (User.IsInRole("Member"))
                {
                    return RedirectToAction("AllNewsMember");
                }

                    return View(model);
            }
            else
            {
                return Unauthorized(); // O cualquier otro código de estado que desees devolver
            }



        }


		public IActionResult EditNews(int id)
		{
            var data = _newsDbContext.News.FirstOrDefault(x => x.NewsId == id);

            if (data == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
            ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");

            var model = new NewsViewModel
            {
                Author = data.Author,
                Title = data.Title,
                Description = data.Description,
                Content = data.Content,
                ImagePath = data.ImagePath,
                PublishedAt = data.PublishedAt,
                CategoryId = data.CategoryId,
                CountryId = data.CountryId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditNews(int id, NewsViewModel model,IFormFile imageFile)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var data = _newsDbContext.News.FirstOrDefault(x => x.NewsId == id);
                if (data != null)
                {
                    data.Author = model.Author;
                    data.Title = model.Title;
                    data.Description = model.Description;
                    data.Content = model.Content;
                    data.PublishedAt = model.PublishedAt;
                    data.CategoryId = model.CategoryId;
                    data.CountryId = model.CountryId;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);
                            data.ImagePath = memoryStream.ToArray();
                        }
                    }

                    _newsDbContext.News.Update(data);
                    _newsDbContext.SaveChanges();
                    ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
                    ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("AllNews");
                    }

                    if (User.IsInRole("Member"))
                    {
                        return RedirectToAction("AllNewsMember");
                    }

                    return View(model);


                }
                else
                {
                    return View(model);
                }


            }
            else
            {
                return Unauthorized(); // O cualquier otro código de estado que desees devolver
            }
        }


        public IActionResult DeleteNews(int id)
        {

            var data = _newsDbContext.News.FirstOrDefault(x => x.NewsId == id);
            if (data == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
            ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");

            var model = new NewsViewModel
            {
                Author = data.Author,
                Title = data.Title,
                Description = data.Description,
                Content = data.Content,
                ImagePath = data.ImagePath,
                PublishedAt = data.PublishedAt,
                CategoryId = data.CategoryId,
                CountryId = data.CountryId
            };

            return View(model);
            
        }

        [HttpPost]
        public IActionResult DeleteNews(int id,NewsViewModel model)
        {
            var userIdClaim = User.FindFirst("UserId");

            if ( userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var data = _newsDbContext.News.FirstOrDefault(x => x.NewsId == id);
                if (data != null)
                {
                    data.Author = model.Author;
                    data.Title = model.Title;
                    data.Description = model.Description;
                    data.Content = model.Content;
                    data.ImagePath = model.ImagePath;
                    data.PublishedAt = model.PublishedAt;
                    data.CategoryId = model.CategoryId;
                    data.CountryId = model.CountryId;
                    _newsDbContext.News.Remove(data);
                    _newsDbContext.SaveChanges();
                    ViewData["Categories"] = new SelectList(_newsDbContext.Categories, "CategoryId", "Name");
                    ViewData["Countries"] = new SelectList(_newsDbContext.Countries, "CountryId", "Name");
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("AllNews");
                    }

                    if (User.IsInRole("Member"))
                    {
                        return RedirectToAction("AllNewsMember");
                    }

                    return View(model);
                }
                else
                {
                    return View(model);
                }

            }
            else
            {
                return Unauthorized(); // O cualquier otro código de estado que desees devolver
            }

        }

        //[Authorize(Roles = "Admin,Member")]
        //public async Task<IActionResult> Category()
        //{
        //    var HttpClient = _httpClientFactory.CreateClient();
        //    var apiResponse = await HttpClient.GetStringAsync("https://localhost:7203/api/News/Category");
        //    var TopHeadlinesResponse = JObject.Parse(apiResponse);

        //    return View(TopHeadlinesResponse);
        //}
    }
}
