using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : 
        Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>, 
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(success: false, message: "Não foi possível realizar sua assinatura");
            }

            // Verificar se Documento já está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotification(nameof(command.Document), "Este CPF já está em uso");
            }

            // Verificar se E-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification(nameof(command.Email), "Este e-mail já está em uso");
            }

            // Gerar os VOs
            var name = new Name(firstName: command.FirstName, lastName: command.LastName);
            var document = new Document(number: command.Document, type: EDocumentType.CPF);
            var email = new Email(address: command.Email);
            var address = new Address(
                street: command.Street,
                number: command.StreetNumber,
                neighborhood: command.Neighborhood,
                city: command.City,
                state: command.State,
                country: command.Country,
                zipCode: command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name: name,document: document,email: email);
            var subscription = new Subscription(expireDate: DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                    paidDate: command.PaidDate,
                    expireDate: command.ExpireDate,
                    total: command.Total,
                    totalPaid: command.TotalPaid,
                    payer: command.Payer,
                    document: new Document(number: command.PayerDocument, type: command.PayerDocumentType),
                    address: address,
                    barCode: command.BarCode, 
                    boletoNumber: command.BoletoNumber,
                    email: new Email(address: command.PayerEmail)
                    );

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);        

            // Aplicar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificações
            if (Invalid)
            {
                return new CommandResult(success: false, message: "Não foi possível realizar sua assinatura");
            }

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar e-mail de boas vindas
            _emailService.Send(
                to: student.Name.ToString(), 
                email: student.Email.Address, 
                subject: "Bem vindo!", 
                body:"Sua assinatura foi criada");

            // Retornar informaçoes

            return new CommandResult(success: true, message: "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            // Verificar se Documento já está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotification(nameof(command.Document), "Este CPF já está em uso");
            }

            // Verificar se E-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification(nameof(command.Email), "Este e-mail já está em uso");
            }

            // Gerar os VOs
            var name = new Name(firstName: command.FirstName, lastName: command.LastName);
            var document = new Document(number: command.Document, type: EDocumentType.CPF);
            var email = new Email(address: command.Email);
            var address = new Address(
                street: command.Street,
                number: command.StreetNumber,
                neighborhood: command.Neighborhood,
                city: command.City,
                state: command.State,
                country: command.Country,
                zipCode: command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name: name,document: document,email: email);
            var subscription = new Subscription(expireDate: DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                    paidDate: command.PaidDate,
                    expireDate: command.ExpireDate,
                    total: command.Total,
                    totalPaid: command.TotalPaid,
                    payer: command.Payer,
                    document: new Document(number: command.PayerDocument, type: command.PayerDocumentType),
                    address: address,
                    transactionCode: command.TransactionCode, 
                    email: new Email(address: command.PayerEmail)
                    );

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);        

            // Aplicar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar e-mail de boas vindas
            _emailService.Send(
                to: student.Name.ToString(), 
                email: student.Email.Address, 
                subject: "Bem vindo!", 
                body:"Sua assinatura foi criada");

            // Retornar informaçoes

            return new CommandResult(success: true, message: "Assinatura realizada com sucesso");
        }
    }
}