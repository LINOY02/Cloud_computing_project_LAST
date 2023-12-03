using System.ComponentModel;

namespace Cloud_computing_project_LAST.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Description("השם שבו יוצג הקפה")]
        [DisplayName("Description")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
