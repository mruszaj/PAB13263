using AdminPanel.Services;
using ApiRest.SoapService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AdminPanel.Pages.Admin
{
    public class ProduktyModel : PageModel
    {
        private readonly SoapProductsClient _soapClient;

        public ProduktyModel(SoapProductsClient soapClient)
        {
            _soapClient = soapClient;
        }

        public List<ProduktDTO> Products { get; set; } = new();
        public List<UzytkownikDTO> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _soapClient.GetProductsAsync();

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
