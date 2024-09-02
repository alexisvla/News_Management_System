using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace NEWS_WebAplication.Models
{
    public class NewsViewModel
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


    }
}
