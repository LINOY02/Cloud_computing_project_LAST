using System.ComponentModel;
using System.Data.Entity;

namespace Cloud_computing_project_LAST.Models
{
    public class Cafe : DbContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Description("השם שבו יוצג הקפה")]
        [DisplayName("תאור הקפה")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}