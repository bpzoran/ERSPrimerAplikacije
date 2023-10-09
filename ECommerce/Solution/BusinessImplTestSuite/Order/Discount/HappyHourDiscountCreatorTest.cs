using Application.Customer.Queries;
using Application.Order.Interfaces;
using BusinessImpl.Order.Discount;
using Domain;
using Moq;
using NUnit.Framework;
using TestFaker;

namespace BusinessImplTestSuite.Order.Discount
{
    public class HappyHourDiscountCreatorTest
    {
        private IDiscountCreator happyHourDiscountCreator;
        private IDiscount fixedDiscount;
        private IDiscount phoneNumberDiscount;
        private Mock<ICustomerFindByIdQuery> customerFindByIdQuery;
        private CustomerEntity customerEntity;
        private OrderEntity orderEntity;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            customerFindByIdQuery = new Mock<ICustomerFindByIdQuery>();
            customerEntity = FakeObjects.GetCustomer();
            orderEntity = FakeObjects.GetOrder(customerEntity);
            customerFindByIdQuery.Setup(t => t.FindById(It.IsAny<object>())).Returns(customerEntity);
            fixedDiscount = new FixedDiscount();
            phoneNumberDiscount = new PhoneNumberEndDiscount(customerFindByIdQuery.Object, "1");
            happyHourDiscountCreator = new HappyHourDiscountCreator(phoneNumberDiscount, fixedDiscount);
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetDiscountEvenPhoneTest()
        {
            customerEntity.PhoneNumber = "+38161689654";
            orderEntity.OrderTime = new System.DateTime(2023, 10, 9, 16, 30, 0);
            Assert.AreEqual(happyHourDiscountCreator.GetDiscount(orderEntity).DiscountPercentage, 20);
        }

        [Test]
        public void GetDiscountOddPhoneTest()
        {
            customerEntity.PhoneNumber = "+38161689655";
            orderEntity.OrderTime = new System.DateTime(2023, 10, 9, 16, 30, 0);
            Assert.AreEqual(happyHourDiscountCreator.GetDiscount(orderEntity).DiscountPercentage, 10);
        }

        [Test]
        public void GetDiscountZeroPhoneTest()
        {
            customerEntity.PhoneNumber = "+38161689650";
            orderEntity.OrderTime = new System.DateTime(2023, 10, 9, 16, 30, 0);
            Assert.AreEqual(happyHourDiscountCreator.GetDiscount(orderEntity).DiscountPercentage, 30);
        }

        [Test]
        public void GetNoDiscountZeroTest()
        {
            customerEntity.PhoneNumber = "+38161689650";
            orderEntity.OrderTime = new System.DateTime(2023, 10, 9, 12, 30, 0);
            Assert.AreEqual(happyHourDiscountCreator.GetDiscount(orderEntity).DiscountPercentage, 0);
        }
    }
}
