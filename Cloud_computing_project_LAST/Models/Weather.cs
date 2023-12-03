namespace Cloud_computing_project_LAST.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public int orderId { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }

    }
}
