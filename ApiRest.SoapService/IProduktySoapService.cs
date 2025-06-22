using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using ApiRest.SoapService;

namespace ApiRest.SoapService
{
    
    public class ProductsSoapService : IProductsSoapService
    {
        private static List<ProduktDTO> _products = new List<ProduktDTO>
        {
            new ProduktDTO { Id = 1, Name = "Monitor", Price = 1000 },
            new ProduktDTO { Id = 2, Name = "Keyboard", Price = 50 }
        };

        public void AddProduct(ProduktDTO product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public ProduktDTO GetProdukt(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public List<ProduktDTO> GetProducts()
        {
            return _products;
        }

        public void RemoveProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
                _products.Remove(product);
        }
    }

    
    [ServiceContract]
    public interface IProductsSoapService
    {
        [OperationContract]
        List<ProduktDTO> GetProducts();

        [OperationContract]
        ProduktDTO GetProdukt(int id);

        [OperationContract]
        void AddProduct(ProduktDTO product);

        [OperationContract]
        void RemoveProduct(int id);
    }

    
    [DataContract]
    public class ProduktDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }
}
