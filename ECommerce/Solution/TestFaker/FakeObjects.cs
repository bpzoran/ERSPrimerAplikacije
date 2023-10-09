using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFaker
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

        public static OrderEntity GetOrder(CustomerEntity customer)
        {
            return new OrderEntity()
            {
                Customer = customer,
                InitialTotalPrice = 1000,
                FinalTotalPrice = 1000,
                OrderTime = new DateTime(2023, 10, 9, 22, 0, 0),
                OrderId = "1",
                ProceedingCity = "Novi Sad",
                ProceedingHouseNumber = "16",
                ProceedingStreet = "Marsala Tita"
            };
        }
    }
}
