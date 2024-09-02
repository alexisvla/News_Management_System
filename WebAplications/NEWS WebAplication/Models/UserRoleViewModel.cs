using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace NEWS_WebAplication.Models
{
    public class UserRoleViewModel
    {
		public int UserRoleId { get; set; }
		[Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [BindNever]
        public List<UserRole> UserRoles { get; set; }
    }
}
