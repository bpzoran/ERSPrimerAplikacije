using Application.Customer.Queries;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    // SOLID explanation
    // Single Resonsibility Principle
    // Umesto da imamo u jednoj klasi celokupnu funkcionalnost u vezi sa popustima,
    // funkcionalnosti smo razbili po klasama.
    // Tako u ovoj klasi imamo samo jednu funkcionalnost koja odredjuje procenat popusta na osnovu broja telefona.
    // Da smo imali u jednoj klasi i odredjivanje popusta na osnovu broja telefona i kreiranje popusta na osnovu vremena,
    // sta bi bilo u situaciji da nam korisnik iznese novi zahtev u vezi sa popustom - na primer obrne se popust u odnosu na parne i neparne krajeve broja telefona?
    // Tada bismo morali da menjamo klasu koja sadrzi nacin odredjivanja popusta na osnovu vremena, iako izmena nema nikakve veze sa tom funkcionalnoscu.
    // Posledicno bi moglo da dodje do gresaka, neki testovi bi mozda popadali itd.
    public class PhoneNumberEndDiscount : IDiscount
    {
        private float discountPercentageDefault = 10f;


        public ICustomerFindByIdQuery CustomerFindByIdQuery { get; set; }

        public float DiscountPercentage { get => GetDiscountPercentage(); set => discountPercentageDefault = value; }

        private string customerId;
        public PhoneNumberEndDiscount(ICustomerFindByIdQuery customerFindByIdQuery, string customerId)
        {
            this.CustomerFindByIdQuery = customerFindByIdQuery;
            this.customerId = customerId;            
        }
        private float GetDiscountPercentage()
        {
            var customer = this.CustomerFindByIdQuery.FindById(customerId);
            string sPhoneNumberEnd = PhoneNumberEnd(customer.PhoneNumber);
            if (!int.TryParse(sPhoneNumberEnd, out int nPhoneNumberEnd))
            {
                return 0;
            }
            if (nPhoneNumberEnd == 0)
            {
                return 30;
            }
            if (nPhoneNumberEnd % 2 == 0)
            {
                return 20;
            }
            return discountPercentageDefault;
        }

        private string PhoneNumberEnd(string phoneNumber)
        {
            return phoneNumber[phoneNumber.Length - 1].ToString();
        }
    }
}
