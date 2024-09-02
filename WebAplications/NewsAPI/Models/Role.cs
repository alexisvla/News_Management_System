using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; }

    public List<UserRole> UserRole { get; set; }
}
