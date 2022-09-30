using IdentityApp.Settings;
using Microsoft.Extensions.Options;

namespace IdentityApp.Services
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}