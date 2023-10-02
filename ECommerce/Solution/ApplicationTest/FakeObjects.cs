using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTest
{
    public class FakeObjects
    {
        public static CustomerEntity GetCustomer()
        {
            return new CustomerEntity()
            {
                City = "Novi Sad",
                CustomerId = "1",
                FirstName = "Petar",
                LastName = "Petrovic",
                PhoneNumber = "+38164123456",
                Street = "Dositeja Obradovica 2"
            };
        }

        public static ProductEntity GetProduct(SupplierEntity supplier)
        {
            return new ProductEntity()
            {
                ProductId = "1",
                ProductName = "T-Shirt",
                ProductPrice = 3000f,
                Supplier = supplier
            };
        }

        public static SupplierEntity GetSupplier()
        {
            return new SupplierEntity()
            {
                SupplierId = "1",
                SupplierName = "Export Imort Commerce"
            };
        }

        public static LocalStockEntity GetLocalStock()
        {
            return new LocalStockEntity()
            {
                StockId = 1,
                StockName = "Default local stock",
                Address = "Fruskogorska 2",
                IsDefault = true,
                Capacity = 10000
            };
        }

        public static SupplierStockEntity GetSupplierStock()
        {
            return new SupplierStockEntity()
            {
                StockId = 2,
                StockName = "Default supplier stock",
                IsDefault = true,
                WebServiceURL = "www.someaddress.com",
                Supplier = GetSupplier()

            };
        }
    }
}
