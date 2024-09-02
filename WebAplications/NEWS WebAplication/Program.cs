using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NEWS_WebAplication.Controllers;
using NEWS_WebAplication.Data;
using NEWS_WebAplication.Services;
using NewsAPI.Data;
using System.Configuration;

namespace NEWS_WebAplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            

            builder.Services.AddDbContext<NewsDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddHttpClient<NewsController>();
            builder.Services.AddHttpClient<AdminController>();

            

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Ruta para la página de inicio de sesión
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.AccessDeniedPath = "/Home/Privacy"; // Ruta para el acceso denegado
                });

            builder.Services.AddScoped<IMailSender,Mailsender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}"); 
            
            app.Run();
        }
    }
}