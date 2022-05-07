using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Address _address;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentTests()
        {
            _name = new Name(firstName: "Bruce", lastName: "Wayne");
            _document = new Document(number: "12345678901", type: EDocumentType.CPF);
            _email = new Email(address: "email@teste.com.br");
            _student = new Student(name: _name, document: _document, email: _email);
            _subscription = new Subscription(null);
            _address = new Address(
                street: "Way Street",
                number: "123",
                neighborhood: "",
                city: "Gotham",
                state: "DC",
                country: "USA",
                zipCode: "12345");

        }

        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            var paypalPayment = new PayPalPayment(
                    paidDate: DateTime.Now,
                    expireDate: DateTime.Now.AddDays(5),
                    total: 10,
                    totalPaid: 10,
                    payer: "Wayne Corp",
                    document: _document,
                    address: _address,
                    transactionCode: "23-ab2",
                    email: _email);
            _subscription.AddPayment(paypalPayment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenAddSubscription()
        {
            var paypalPayment = new PayPalPayment(
                    paidDate: DateTime.Now,
                    expireDate: DateTime.Now.AddDays(5),
                    total: 10,
                    totalPaid: 10,
                    payer: "Wayne Corp",
                    document: _document,
                    address: _address,
                    transactionCode: "23-ab2",
                    email: _email);
            _subscription.AddPayment(paypalPayment);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }
    }
}
