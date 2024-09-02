using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string password { get; set; }
    public string DisplayName { get; set; }
    public List<UserRole> UserRole { get; set; }

    [JsonIgnore]
    public virtual ICollection<News> News { get; set; } = new List<News>();
}
