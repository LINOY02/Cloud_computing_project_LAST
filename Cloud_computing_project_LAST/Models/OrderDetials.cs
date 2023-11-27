namespace Cloud_computing_project_LAST.Models
{
    public class OrderDetials
    {
        public int Id { get; set; } 

        public WeatherService? weather { get; set; }
        public HebcalService? hebcal { get; set; }
        public List<CartItem>? CartItem { get; set; }
    }
}
