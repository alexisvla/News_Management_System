using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NEWS_WebAplication.Models;
using NewsAPI.Data;
using NewsAPI.Models;
using System.Security.Claims;

namespace NEWS_WebAplication.Controllers
{
    public class UserRoleController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5116/api");
        private readonly HttpClient _httpClient;
        private readonly NewsDbContext _newsDbContext;

        public UserRoleController(HttpClient httpClient, NewsDbContext newsDbContext)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
            _newsDbContext = newsDbContext;
        }

        public IActionResult AllUsers()
        {

            var userRoles = _newsDbContext.UserRoles.Include(ur => ur.user).Include(ur => ur.role).ToList();
            var viewModel = new UserRoleViewModel
            {
                UserRoles = userRoles
            };

            return View(viewModel);
        }


        public IActionResult AssignRoles()
        {

            ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
            ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");
            return View(new UserRoleViewModel());
        }

        [HttpPost]
        public IActionResult AssignRoles(UserRoleViewModel model)
        {
            if (model != null && model.UserId > 0 && model.RoleId > 0)
            {
                var User = new UserRole();
				User.UserId = model.UserId;
				User.RoleId = model.RoleId;
				_newsDbContext.Add(User);
				_newsDbContext.SaveChanges();

				ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
				ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");
				return RedirectToAction("AllUsers");
			}
			return View(model);     


		}


        public IActionResult EditRoles(int id)
        {
            var data = _newsDbContext.UserRoles.FirstOrDefault(x => x.UserRoleId == id);

            if (data == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
            ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");

            var model = new UserRoleViewModel
            {
                UserId = data.UserId,
                RoleId = data.RoleId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditRoles(int id, UserRoleViewModel model)
        {

			var existingUserRole = _newsDbContext.UserRoles.FirstOrDefault(x => x.UserRoleId == id);

			if (existingUserRole == null)
			{
				return NotFound();
			}

			var updatedUserRole = new UserRole
			{
				UserId = model.UserId,
				RoleId = model.RoleId
			};

			_newsDbContext.UserRoles.Remove(existingUserRole); // Eliminar el objeto existente
			_newsDbContext.UserRoles.Add(updatedUserRole); // Agregar el objeto actualizado
			_newsDbContext.SaveChanges();

			ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
			ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");

			return RedirectToAction("AllUsers");
			


		}


        public IActionResult DeleteRoles(int id)
        {

            var data = _newsDbContext.UserRoles.SingleOrDefault(x => x.UserRoleId == id);
            if (data == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
            ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");

            var model = new UserRoleViewModel
            {
				UserId = data.UserId,
				RoleId = data.RoleId
			};

            return View(model);

        }

        [HttpPost]
        public IActionResult DeleteRoles(int id, UserRoleViewModel model)
        {

            var data = _newsDbContext.UserRoles.FirstOrDefault(x => x.UserRoleId == id);
            if (data != null)
            {
                data.UserId = model.UserId;
                data.RoleId = model.RoleId;

                _newsDbContext.UserRoles.Remove(data);
                _newsDbContext.SaveChanges();
                ViewData["Users"] = new SelectList(_newsDbContext.Users, "UserId", "DisplayName");
                ViewData["Roles"] = new SelectList(_newsDbContext.Roles, "RoleId", "Name");
                return RedirectToAction("AllUsers");
            }
            else
            {
                return View(model);
            }



        }
    }
}
