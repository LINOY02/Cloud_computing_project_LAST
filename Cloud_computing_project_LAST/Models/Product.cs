using System.ComponentModel;

namespace Cloud_computing_project_LAST.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int InStock { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }        
    }
}
