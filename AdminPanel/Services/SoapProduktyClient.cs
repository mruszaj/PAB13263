using ApiRest.SoapService;

namespace AdminPanel.Services
{
    public class SoapProductsClient
    {
        private readonly IProductsSoapService _soap;

        public SoapProductsClient(IProductsSoapService soap)
        {
            _soap = soap;
        }

        public Task<List<ProduktDTO>> GetProductsAsync()
        {
         
            return Task.FromResult(_soap.GetProducts());
        }
    }
}
