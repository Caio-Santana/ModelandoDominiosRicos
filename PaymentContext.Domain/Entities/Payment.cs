using System;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public abstract class Payment : Entity
    {
        protected Payment(DateTime paidDate, DateTime expireDate, decimal total, decimal totalPaid, string payer, Document document, Address address)
        {
            Number = Guid.NewGuid()
                .ToString()
                .Replace("-","")
                .Substring(0,10)
                .ToUpper();
            PaidDate = paidDate;
            ExpireDate = expireDate;
            Total = total;
            TotalPaid = totalPaid;
            Payer = payer;
            Document = document;
            Address = address;

            AddNotifications(
                new Contract()
                .Requires()
                .IsLowerOrEqualsThan(0, Total, nameof(Total), "O total não pode ser zero")
                .IsGreaterOrEqualsThan(Total, TotalPaid, nameof(TotalPaid), "O valor pago é menor que o valor total à pagar")
            );
        }

        public string Number { get; private set; }
        public DateTime PaidDate { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public decimal Total { get; private set; }
        public decimal TotalPaid { get; private set; }
        public string Payer { get; private set; }
        public Document Document { get; private set; }
        public Address Address { get; private set; }
    }
}