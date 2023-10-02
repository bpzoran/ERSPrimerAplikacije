using Domain;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class HappyHourDiscountCreator : FixedDiscountCreator, IDiscountCreator
    {
        private IDiscount Discount;

        public HappyHourDiscountCreator(IDiscount discount, IDiscount noDiscount) : base(noDiscount) {
            this.Discount = discount;
        }

        public new IDiscount GetDiscount(OrderEntity order)
        {
            var orderTime = order.OrderTime;
            if (orderTime.Hour >= 16 && orderTime.Hour <= 17)
            {
                return Discount;
            }
            return base.GetDiscount(order);
        }
    }
}
