namespace Pronia.Abstractions.Services
{
    public interface IEmailService
    {
        public void Send(string mailTo, string subject, string body, bool IsHtml = false);
    }
}
