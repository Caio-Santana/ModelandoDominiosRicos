using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // Red, Green, Refactor

        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var fakeStudentRepo = new FakeStudentRepository();
            var fakeEmailService = new FakeEmailService();
            var handler = new SubscriptionHandler(fakeStudentRepo, fakeEmailService);
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "99999999999";
            command.Email = "outro@email.com";   
            command.BarCode = "123456"; 
            command.BoletoNumber = "123456"; 
            command.PaymentNumber = "12345";
            command.PaidDate = DateTime.Now; 
            command.ExpireDate = DateTime.Now.AddMonths(1); 
            command.Total = 10m; 
            command.TotalPaid = 10m; 
            command.Payer = "Wayne Corp"; 
            command.PayerDocument = "12345678901"; 
            command.PayerDocumentType = EDocumentType.CPF; 
            command.PayerEmail = "email@pgto.com"; 
            command.Street = "St way"; 
            command.StreetNumber = "123"; 
            command.Neighborhood  = "none";
            command.City = "Gotham"; 
            command.State = "DC"; 
            command.Country = "USA"; 
            command.ZipCode = "12345"; 

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
    }


}
}
