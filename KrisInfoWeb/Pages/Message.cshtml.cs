using KrisInfoWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KrisInfoWeb.Pages
{
    public class MessageModel : PageModel
    {
        public MessageModel()
        {

        }
        public KrisInfoResponse Information { get; set; }

        public async Task OnGetAsync(int id)
        {
            using (HttpClient client = new())
            {
                var days = 365;
                client.BaseAddress = new Uri("https://api.krisinformation.se");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/v3/news/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // G�r om responsen till en str�ng
                    var responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        // G�r om str�ngen till v�r egen skapade datatyp - KrisInfoResponse
                        Information = JsonConvert.DeserializeObject<KrisInfoResponse>(responseBody);

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
