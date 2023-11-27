namespace Cloud_computing_project_LAST.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int InStock { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }


        public CartItem()
        {
            Id = 123;
            Name = "coffee";
            Description = "good";
            InStock = 5;
            ImageUrl = "ayala";
            Price = 25;
            Amount = 3;
        }


    }
}
