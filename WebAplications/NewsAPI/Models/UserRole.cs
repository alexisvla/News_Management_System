using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsAPI.Models;

public partial class UserRole
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public User user { get; set; }
    public Role role { get; set; }
}
