using System.Net;
using System.Net.Mail;
using Core.Configs;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class SmtpClientWrapper : ISmtpClient
{
    private bool _disposed;
    private readonly SmtpClient _smtpClient;

    public SmtpClientWrapper(IOptions<AppConfig> config)
    {
        var fromAddress = new MailAddress(config.Value.SmtpConfig.EmailAddress, config.Value.SmtpConfig.Name);
        var credentials = new NetworkCredential(fromAddress.Address, config.Value.SmtpConfig.Password);

        _smtpClient = new SmtpClient
        {
            Host = config.Value.SmtpConfig.Host,
            Port = config.Value.SmtpConfig.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = credentials
        };
    }

    ~SmtpClientWrapper()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _smtpClient.Dispose();
            }
            _disposed = true;
        }
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SmtpClientWrapper));
        }
    }

    public Task SendMailAsync(MailMessage mailMessage)
    {
        CheckDisposed();
        return _smtpClient.SendMailAsync(mailMessage);
    }
}