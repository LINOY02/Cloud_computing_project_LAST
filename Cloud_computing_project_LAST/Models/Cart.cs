using System.ComponentModel.DataAnnotations;
namespace Cloud_computing_project_LAST.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Cafe? Cafe { get; set; }
        public int OrderId { get; set; }
    }
}
