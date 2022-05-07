using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class BoletoPayment : Payment
    {
        public BoletoPayment(DateTime paidDate, DateTime expireDate, decimal total, decimal totalPaid, string payer, Document document, Address address, string barCode, string boletoNumber, Email email) : base(paidDate, expireDate, total, totalPaid, payer, document, address)
        {
            BarCode = barCode;
            BoletoNumber = boletoNumber;
            Email = email;
        }

        public string BarCode { get; private set; }
        public string BoletoNumber { get; private set; }
        public Email Email { get; private set; }
    }
}