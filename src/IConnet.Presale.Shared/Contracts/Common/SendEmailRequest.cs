namespace IConnet.Presale.Shared.Contracts.Common;

public record SendEmailRequest(string EmailAddressTo, string EmailAddressFrom, string Password,
        string Subject, string Body, string SmtpHost, int Port);
