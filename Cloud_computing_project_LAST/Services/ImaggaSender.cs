using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloud_computing_project_LAST.Services
{
    public class ImaggaSender
    {
        private readonly HttpClient _httpClient;

        public ImaggaSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.imagga.com/v2/"); // Set the base address
        }

        public async Task<bool> CheckImage(string imageUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync($"tags?image_url={imageUrl}");

                response.EnsureSuccessStatusCode();

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var result = await JsonSerializer.DeserializeAsync<ImaggaResponse>(responseStream);

                    // Process the result and return true or false based on your logic
                    // You can use result.Tags for the list of tags

                    return false; // Placeholder, replace with your logic
                }
            }
            catch (HttpRequestException)
            {
                // Handle request exception
                return false;
            }
        }
    }
}
