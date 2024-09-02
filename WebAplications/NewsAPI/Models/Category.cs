using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NewsAPI.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<News> News { get; set; } = new List<News>();
}
