using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using TechVeo.Notification.Application.Services;

namespace TechVeo.Notification.Infra.Services;

public class SendEmailService : ISendEmailService
{
    private readonly ILogger<SendEmailService> _logger;

    public SendEmailService(ILogger<SendEmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toAddress, string subject, string bodyHtml)
    {
        var fromAddress = "seu-email@verificado.com";

        using (var client = new AmazonSimpleEmailServiceClient("SEU_ACCESS_KEY", "SEU_SECRET_KEY", RegionEndpoint.USEast1))
        {
            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination { ToAddresses = new() { toAddress } },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body { Html = new Content(bodyHtml) }
                }
            };

            try
            {
                var response = await client.SendEmailAsync(sendRequest);
                Console.WriteLine("Email enviado com sucesso! ID: " + response.MessageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar: " + ex.Message);
            }
        }
    }
}
