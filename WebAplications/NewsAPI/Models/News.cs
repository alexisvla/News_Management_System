using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string Author { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Content { get; set; } = null!;

    public byte[]? ImagePath { get; set; } 

    public DateTime PublishedAt { get; set; }

    public int CategoryId { get; set; }

    public int CountryId { get; set; }

    public int UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
