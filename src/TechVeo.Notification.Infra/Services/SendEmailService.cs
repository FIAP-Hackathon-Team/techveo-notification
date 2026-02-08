using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechVeo.Notification.Application.Services;
using TechVeo.Shared.Application.Aws;

namespace TechVeo.Notification.Infra.Services;

public class SendEmailService : ISendEmailService
{
    private readonly ILogger<SendEmailService> _logger;
    private readonly AmazonSimpleEmailServiceClient _client;
    private readonly string _fromAddress;

    public SendEmailService(ILogger<SendEmailService> logger, IOptions<AwsOptions> awsOptions, IConfiguration configuration)
    {
        _logger = logger;

        _fromAddress = configuration["Email:FromAddress"]!;

        _client = new AmazonSimpleEmailServiceClient(awsOptions.Value.AccessKey, awsOptions.Value.SecretKey,
                  RegionEndpoint.GetBySystemName(awsOptions.Value.Region));
    }

    public async Task SendAsync(string toAddress, string subject, string bodyHtml)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = _fromAddress,
            Destination = new Destination { ToAddresses = new() { toAddress } },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body { Html = new Content(bodyHtml) }
            }
        };

        try
        {
            var response = await _client.SendEmailAsync(sendRequest);
            Console.WriteLine("Email enviado com sucesso! ID: " + response.MessageId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar: " + ex.Message);
        }
    }
}
