using Domain;
using Domain.Results;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;
using Application.Common.Factory;
using Application.Customer.Queries;
using Application.Customer.Commands;
using Application.Product.Queries;
using Application.Stock.LocalStock.Queries;
using Application.Stock.SupplierStock.Queries;

namespace Application.Order.Handlers
{
    // SOLID Explanation: Dependency Inversion Principle
    // High-level modules should not  depend on low-level modules.	Both  should depend on abstractions
    // Ovo (Application projekat) je alikativni deo kome cijim metodama direktno pristupa klijent (na primer web ili desktop). Mozemo da kazemo da je ovo High-Level modul.
    // Sa druge strane imamo BusinessImpl projekat koji prema toku podataka sledi posle  Application projekta, tako da mozemo da kazemo da je u pitanju Low-Level modul.
    // A imamo i sloj prezistencije (RepoInMemory), koji je jos vide Low-Level modul.
    // Prema klasicnoj arhitekturi, Application bi zavisio od BusinessImpl i od RepoInMemory jer ih koristi. Medjutim prema cistoj arhitekturi to nije slucaj. 
    // Na priemr ova klasa ne zavisi ni od jedne klase iz BusineeImpl ilki RepoInMemory. Ona zavisi samo od apstrakcija (interfejsi ILocalStockGetDefaultLocalStockQuery,
    // ICustomerFindByIdQuery, IStockWithdrawHandler...). Koja ce tacno ipmplementacija biti koriscena to ce odrediti klijentski deo aplikacije (na primer Web ili Desktop).
    // Tako moze da se injectuje bilo koja druga implementacija i da se aplikacija menja u skladu sa potrebama, a time ce odrzavanje biti lakse, a ceo aplikativni sistem stabilniji.
    public class AddToShoppingCartHandler
    {

        public ICustomerFindByIdQuery CustomerFindByIdQuery { private get; set; }
        public ICustomerUpdateCommand CustomerUpdateCommand { private get; set; }
        public IProductFindByIdQuery ProductFindByIdQuery { private get; set; }
        public ILocalStockGetDefaultLocalStockQuery StockGetDefaultLocalStockQuery { private get; set; }
        public ISupplierStockGetDefaultSupplierStockQuery SupplierStockGetDefaultSupplierStockQuery { private get; set; }
        public IStockWithdrawHandler CommonStockWithdrawHandler { private get; set; }
        public IStockAvailabilityChecker StockAvailabilityChecker { private get; set; }

        public AddToShoppingCartHandler(
            IRepoFactory repoFactory,
            IStockWithdrawHandler commonStockWithdrawHandler,
            IStockAvailabilityChecker stockAvailabilityChecker
            ) : base()
        {
            this.CustomerFindByIdQuery = repoFactory.CustomerFindByIdQuery;
            this.CustomerUpdateCommand = repoFactory.CustomerUpdateCommand;
            this.ProductFindByIdQuery = repoFactory.ProductFindByIdQuery;
            this.StockGetDefaultLocalStockQuery = repoFactory.LocalStockGetDefaultLocalStockQuery;
            this.SupplierStockGetDefaultSupplierStockQuery = repoFactory.SupplierStockGetDefaultSupplierStockQuery;
            this.CommonStockWithdrawHandler = commonStockWithdrawHandler;
            this.StockAvailabilityChecker = stockAvailabilityChecker;
        }

        public Result AddToShoppingCart(string productId, string customerId, float quantity)
        {
            Result result = new Result();
            var customer = CustomerFindByIdQuery.FindById(customerId);
            if (customer is null)
            {
                result.Log(LogLevel.Error, $"Unknown customer");
                return result;
            }
            ProductEntity product = ProductFindByIdQuery.FindById(productId);
            SupplierEntity supplier = product.Supplier;
            SupplierStockEntity supplierStock = SupplierStockGetDefaultSupplierStockQuery.GetDefaultSupplierStock(supplier);
            var localStock = StockGetDefaultLocalStockQuery.GetDefaultLocalStock();
            if (StockAvailabilityChecker.CheckAvailability(product, localStock, supplierStock, quantity, result))
            {
                CommonStockWithdrawHandler.WithdrawProduct(product, quantity, localStock, supplierStock, out float missingQuantity, result);
                if (missingQuantity > 0)
                {
                    result.Log(LogLevel.Error, $"Missing quantity for the product: {missingQuantity}");
                }
                if (result.Success)
                {
                    customer.ShoppingCart.Add(new ProductItem(product, quantity));
                    CustomerUpdateCommand.Execute(customer);
                }
            }
            result.ResultObject = customer.ShoppingCart;
            return result;
        }
    }
}
