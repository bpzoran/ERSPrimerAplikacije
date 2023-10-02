using Domain;
using Domain.Helpers;
using Application.Order.Interfaces;
using IBusinessImpl.Order;
using Application.Common.Interfaces;
using System.Linq;
using Application.Customer.Queries;

namespace BusinessImpl.Order
{
    public class CommonOrderCreator : IOrderCreator
    {
        protected CustomerEntity customer;
        public ICustomerFindByIdQuery CustomerFindByIdQuery { private get; set; }
        protected ProceedingData proceedingData;        
        protected IDiscountCreator discountCreator;
        protected ITimeAssigner timeAssigner;

        public CommonOrderCreator(ICustomerFindByIdQuery customerFindByIdQuery, IDiscountCreator discountCreator, ITimeAssigner timeAssigner, ProceedingData proceedingData)
        {
            this.CustomerFindByIdQuery = customerFindByIdQuery;
            this.timeAssigner = timeAssigner;
            this.proceedingData = proceedingData;
            this.discountCreator = discountCreator;
            ApplyProceedingData();
        }

        private void ApplyProceedingData()
        {
            customer = CustomerFindByIdQuery.FindById(proceedingData.ProceedingCustomerId);
            if (string.IsNullOrEmpty(proceedingData.ProceedingCity))
            {
                proceedingData.ProceedingCity = customer.City;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingHouseNumber))
            {
                proceedingData.ProceedingHouseNumber = customer.HouseNumber;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingPhoneNumber))
            {
                proceedingData.ProceedingPhoneNumber = customer.PhoneNumber;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingStreet))
            {
                proceedingData.ProceedingStreet = customer.Street;
            }
        }

        public OrderEntity ApplyShoppingCart()
        {
            OrderEntity order = new OrderEntity
            {
                OrderTime = timeAssigner.DateTime
            };
            ApplyProductItems(order);
            ApplyOrderHeader(order);
            return order;
        }

        private void ApplyOrderHeader(OrderEntity order)
        {
            order.Customer = customer;
            ApplyDelivery(order);
            order.InitialTotalPrice = this.customer.ShoppingCart.Sum(t => t.UnitPrice * t.Quantity);            
            order.FinalTotalPrice = GetFinalTotalPrice(order);
        }

        private void ApplyProductItems(OrderEntity order)
        {
            order.Items.AddRange(this.customer.ShoppingCart);
        }

        private void ApplyDelivery(OrderEntity order)
        {
            order.ProceedingCity = this.proceedingData.ProceedingCity;
            order.ProceedingHouseNumber = this.proceedingData.ProceedingHouseNumber;
            order.ProceedingPhoneNumber = this.proceedingData.ProceedingPhoneNumber;
            order.ProceedingStreet = this.proceedingData.ProceedingStreet;
        }

        private float GetFinalTotalPrice(OrderEntity order)
        {
            order.DiscountPercentage = this.discountCreator.GetDiscount(order).DiscountPercentage;
            if (order.DiscountPercentage == 0f)
            {
                return order.InitialTotalPrice;
            }
            return (1f - order.DiscountPercentage / 100f) * order.InitialTotalPrice;
        }
    }
}
