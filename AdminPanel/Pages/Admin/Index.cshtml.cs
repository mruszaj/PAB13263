using AdminPanel.Services;
using ApiRest.SoapService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AdminPanel.Pages.Admin
{
    [Authorize(Policy = "RequireAdmin")]
    public class IndexModel : PageModel
    {
        private readonly SoapProductsClient _client;

        public IndexModel(SoapProductsClient client)
        {
            _client = client;
        }

        public List<ProduktDTO> Products { get; set; }
        public List<UzytkownikDTO> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _client.GetProductsAsync();

            using var http = new HttpClient();
            var url = "http://localhost:5000/api/User";
            Users = await http.GetFromJsonAsync<List<UzytkownikDTO>>(url);
        }

        public class UzytkownikDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
