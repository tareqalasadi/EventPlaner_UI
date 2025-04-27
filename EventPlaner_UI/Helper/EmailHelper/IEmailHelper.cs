using System.Net.Mail;

namespace EventPlaner_UI.Helper
{
    public interface IEmailHelper
    {
        bool SendEmail(string toEmail, string subject, string body, bool isHtml = true);
        //bool SendEmail(List<string> receiversEmails, string subject, string body, bool _isBodyHtml = true, AlternateView logo = null);
    }
}
