using Domain;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    // SOLID Explanation
    // Liskov Substitution Principle
    // HappyHourDiscountCreator nasledjuje FixedDiscountCreator koji (nije u pitanju apstraktna klasa).
    // Prepoznali smo potrebu za nasledjivanjem jer HappyHourDiscountCreator radi prakticno isto sto i FixedDiscountCreator, osim u specificnom slucaju kada
    // je vreme izmedju 16 i 17 casova.
    // FixedDiscountCreator i HappyHourDiscountCreator implementiraju isti interfejs (IDiscountCreator).
    // Imaju isti public metod (GetDiscount).
    // Obe klase mogu da budu koriscene podjednako, u zavisnosti od potreba.
    // Kada pravimo kompoziciju (dependenci injection), mozemo da izaberemo koju od ove dve klase biramo.
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
