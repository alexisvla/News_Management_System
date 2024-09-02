using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS_WebAplication.Data;
using NEWS_WebAplication.Models.Account;

namespace NEWS_WebAplication.Controllers
{
    public class AdminController : Controller
    {
        //private readonly NewsAPIdbContext _dbContext;
        //private readonly RoleManager<Role> _roleManager;

        //public AdminController(NewsAPIdbContext dbContext, RoleManager<Role> roleManager)
        //{
        //    _dbContext = dbContext;
        //    _roleManager = roleManager;
        //}

        //// Método para mostrar usuarios y sus roles
        //public IActionResult ManageUserRoles()
        //{
        //    var model = new ManageUserRolesViewModel
        //    {
        //        Users = _dbContext.Users.Include(u => u.UserRole).ToList(),
        //        Roles = _roleManager.Roles.ToList()
        //    };
        //    return View(model);
        //}

        //// Método para procesar el formulario de actualización de roles
        //[HttpPost]
        //public IActionResult ManageUserRoles(ManageUserRolesViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _dbContext.Users.Include(u => u.UserRole).FirstOrDefault(u => u.UserId == model.SelectedUserId);
        //        if (user != null)
        //        {
        //            // Actualizar roles del usuario
        //            user.UserRole.Clear(); // Eliminar roles existentes

        //            foreach (var roleId in model.SelectedRoleIds)
        //            {
        //                user.UserRole.Add(new UserRole { UserId = user.UserId, RoleId = roleId });
        //            }

        //            _dbContext.SaveChanges();

        //            return RedirectToAction("ManageUserRoles");
        //        }
        //    }

        //    // Si hay un error, volver a cargar la vista con el modelo
        //    model.Users = _dbContext.Users.Include(u => u.UserRole).ToList();
        //    model.Roles = _roleManager.Roles.ToList();
        //    return View(model);
        //}
    }
}
