using System.Net.Http.Headers;

namespace Cloud_computing_project_LAST.Models
{
    public class AddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5229/") 
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> IsValidAddres(string city, string street)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/address/check?CityName={city}&StreetName={street}");

            if (response.IsSuccessStatusCode)
            {
                var check = response.Content.ReadAsStringAsync();
                if (check.Result == "true")
                    return true;
            }
            return false;
        }
    }
}
