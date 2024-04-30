using KrisInfoWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KrisInfoWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public List<KrisInfoResponse> Information { get; set; }

        public async Task OnGetAsync()
        {
            using (HttpClient client = new())
            {
                var days = 365;
                client.BaseAddress = new Uri("https://api.krisinformation.se");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/v3/news?days={days}");
                if (response.IsSuccessStatusCode)
                {
                    // Gör om responsen till en sträng
                    var responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        // Gör om strängen till vår egen skapade datatyp - KrisInfoResponse
                        Information = JsonConvert.DeserializeObject<List<KrisInfoResponse>>(responseBody);

                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Prutt! Det funkade inte.");
                    }
                }
            }
        }
    }
}