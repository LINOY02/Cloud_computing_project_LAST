namespace Cloud_computing_project_LAST.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public List<CartItem>? CartItem { get; set; }
    }
}
