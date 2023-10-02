using Domain;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class FixedDiscountCreator : IDiscountCreator
    {
        private IDiscount discount;

        public FixedDiscountCreator(IDiscount noDiscount) : base()
        {
            this.discount = noDiscount;
        }

        public IDiscount GetDiscount(OrderEntity order)
        {
            discount.DiscountPercentage = order.DiscountPercentage;
            return discount;
        }
    }
}
