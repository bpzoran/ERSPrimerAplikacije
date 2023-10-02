using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class FixedDiscount : IDiscount
    {
        
        public FixedDiscount()
        {
            this.DiscountPercentage = 0f;
        }

        public float DiscountPercentage { get; set; }

    }
}
