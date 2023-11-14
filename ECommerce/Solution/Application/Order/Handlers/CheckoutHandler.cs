using Domain;
using Domain.Helpers;
using Domain.Results;
using IBusinessImpl.Order;
using Application.Common.Interfaces;
using Application.Customer.Commands;
using Application.Order.Commands;
using Application.Common.Factory;

namespace Application.Order.Handlers
{
    public class CheckoutHandler
    {
        public ICustomerInsertOrUpdateCommand CustomerInsertOrUpdateCommand { private get; set; }

        public ICustomerUpdateCommand CustomerUpdateCommand { private get; set; }
        public IOrderInsertCommand OrderInsertCommand { private get; set; }
        public IOrderCreator OrderCreator { private get; set; }
        public ITimeAssigner TimeAssigner { private get; set; }
        public ProceedingData ProceedingData { private get; set; }
        private readonly CustomerEntity customer;

        public CheckoutHandler(ICustomerInsertOrUpdateCommand customerInsertOrUpdateCommands,
            ICustomerUpdateCommand customerUpdateCommand,
            IOrderInsertCommand orderInsertCommand,
            IOrderCreator orderCreator, 
            ITimeAssigner timeAssigner, 
            ProceedingData proceedingData)
        {
            CustomerInsertOrUpdateCommand = customerInsertOrUpdateCommands;
            CustomerUpdateCommand = customerUpdateCommand;
            OrderInsertCommand = orderInsertCommand;
            this.TimeAssigner = timeAssigner;
            this.OrderCreator = orderCreator;
            this.ProceedingData = proceedingData;
            this.customer = new CustomerEntity(ProceedingData);
            CustomerInsertOrUpdateCommand.Execute(this.customer);
            this.TimeAssigner = timeAssigner;            
        }


        public Result Checkout()
        {
            Result result = new Result();
            if (OrderCreator == null)
            {
                result.Log(LogLevel.Error, $"Unknown customer or invalid input parameters");
                return result;
            }
            OrderEntity order = OrderCreator.ApplyShoppingCart();
            if (!OrderInsertCommand.Execute(order))
            {
                result.Log(LogLevel.Error, $"Unsuccesful storing to db");
            } else
            {
                customer.ShoppingCart.Clear();
                CustomerUpdateCommand.Execute(customer);
            }

            result.ResultObject = order;
            return result;
        }
    }
}
