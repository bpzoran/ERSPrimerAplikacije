using Application.Common.Factory;
using Application.Customer.Commands;
using Application.Customer.Queries;
using Application.Order.Handlers;
using Application.Product.Queries;
using Application.Stock.Interfaces;
using Application.Stock.LocalStock.Queries;
using Application.Stock.SupplierStock.Queries;
using Domain;
using Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTest.Order.Handlers
{
    public class AddToShoppingCartHandlerTest
    {
        private Mock<ICustomerFindByIdQuery> customerFindByIdQueryMock;
        private Mock<ICustomerUpdateCommand> customerUpdateCommandMock;
        private Mock<IProductFindByIdQuery> productFindByIdQueryMock;
        private Mock<ILocalStockGetDefaultLocalStockQuery> localStockGetDefaultLocalStockQueryMock;
        private Mock<ISupplierStockGetDefaultSupplierStockQuery> supplierStockGetDefaultSupplierStockQueryMock;
        private Mock<IStockWithdrawHandler> commonStockWithdrawHandlerMock;
        private Mock<IStockAvailabilityChecker> stockAvailabilityCheckerMock;
        private Mock<IRepoFactory> repoFactoryMock;
        private CustomerEntity customer;
        private ProductEntity product;
        private SupplierEntity supplier;
        private LocalStockEntity localStock;
        private SupplierStockEntity supplierStock;

        private ProductStock GetProductStockLocal(ProductEntity product, LocalStockEntity stock)
        {
            return new ProductStock(product, stock, 54f);
        }

        private ProductStock GetProductStockSupplier(ProductEntity product, SupplierStockEntity supplierStock)
        {
            return new ProductStock(product, supplierStock, 2345f);
        }


        [SetUp]
        public void Setup()
        {
            supplier = FakeObjects.GetSupplier();
            customer = FakeObjects.GetCustomer();
            product = FakeObjects.GetProduct(supplier);            
            localStock = FakeObjects.GetLocalStock();
            supplierStock = FakeObjects.GetSupplierStock();
            supplierStock.ProductStocks = new List<ProductStock>() { GetProductStockSupplier(product, supplierStock) };
            localStock.ProductStocks = new List<ProductStock>() { GetProductStockLocal(product, localStock) };
            customerFindByIdQueryMock = new Mock<ICustomerFindByIdQuery>();
            customerFindByIdQueryMock.Setup(t => t.FindById(It.IsAny<object>())).Returns(customer);
            customerUpdateCommandMock = new Mock<ICustomerUpdateCommand>();
            customerUpdateCommandMock.Setup(t => t.Execute(It.IsAny<CustomerEntity>())).Returns(true);
            productFindByIdQueryMock = new Mock<IProductFindByIdQuery>();
            productFindByIdQueryMock.Setup(t => t.FindById(It.IsAny<object>())).Returns(product);
            localStockGetDefaultLocalStockQueryMock = new Mock<ILocalStockGetDefaultLocalStockQuery>();
            localStockGetDefaultLocalStockQueryMock.Setup(t => t.GetDefaultLocalStock()).Returns(localStock);
            supplierStockGetDefaultSupplierStockQueryMock = new Mock<ISupplierStockGetDefaultSupplierStockQuery>();
            supplierStockGetDefaultSupplierStockQueryMock.Setup(t => t.GetDefaultSupplierStock(It.IsAny<SupplierEntity>())).Returns(supplierStock);
            float missingQuantity = 0f;
            Result res = new Result();
            commonStockWithdrawHandlerMock = new Mock<IStockWithdrawHandler>();
            commonStockWithdrawHandlerMock.Setup(t => t.WithdrawProduct(It.IsAny<ProductEntity>(), It.IsAny<float>(), It.IsAny<LocalStockEntity>(), It.IsAny<SupplierStockEntity>(), out missingQuantity, res));
            stockAvailabilityCheckerMock = new Mock<IStockAvailabilityChecker>();
            stockAvailabilityCheckerMock.Setup(t => t.CheckAvailability(It.IsAny<ProductEntity>(), It.IsAny<LocalStockEntity>(), It.IsAny<SupplierStockEntity>(), It.IsAny<float>(), It.IsAny<Result>())).Returns(true);
            repoFactoryMock = new Mock<IRepoFactory>();
            repoFactoryMock.Setup(t => t.CustomerFindByIdQuery).Returns(customerFindByIdQueryMock.Object);
            repoFactoryMock.Setup(t => t.CustomerUpdateCommand).Returns(customerUpdateCommandMock.Object);
            repoFactoryMock.Setup(t => t.ProductFindByIdQuery).Returns(productFindByIdQueryMock.Object);
            repoFactoryMock.Setup(t => t.LocalStockGetDefaultLocalStockQuery).Returns(localStockGetDefaultLocalStockQueryMock.Object);
            repoFactoryMock.Setup(t => t.SupplierStockGetDefaultSupplierStockQuery).Returns(supplierStockGetDefaultSupplierStockQueryMock.Object);

        }

        [Test]
        public void AddToShoppingCart()
        {
            var testObject = new AddToShoppingCartHandler(repoFactoryMock.Object, commonStockWithdrawHandlerMock.Object, stockAvailabilityCheckerMock.Object);
            var res = testObject.AddToShoppingCart("1", "1", 5f);
            Assert.AreEqual(res.ResultObject.GetType(), typeof(ShoppingCartEntity));
            Assert.AreEqual(((ShoppingCartEntity)(res.ResultObject)).Count, 1);
            Assert.AreEqual(((ShoppingCartEntity)(res.ResultObject))[0].Product, product);
            Assert.IsTrue(res.Success);
        }
    }
}
