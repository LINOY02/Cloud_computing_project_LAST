namespace Cloud_computing_project_LAST.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double TotalPrice { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        //public WeatherResponse weatherResponse { get; set; }
        //public HebcalResponse hebcalResponse { get; set; }
    }
}
