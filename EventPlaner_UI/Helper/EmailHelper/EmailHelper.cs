using System;
using System.Net;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Configuration;
namespace EventPlaner_UI.Helper

{
   

    public class EmailHelper: IEmailHelper
    {
        private readonly IConfiguration _configuration;

        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //public    bool SendEmail(List<string> receiversEmails, string subject, string body, bool _isBodyHtml = true, AlternateView logo = null)
        //{
        //    #region Inject IConfiguration 
        //    IConfiguration configuration = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json")
        //                .AddJsonFile("appsettings.Development.json", optional: true)
        //                .Build();

        //    #endregion

        //    AlternateView alternateView = null;
        //    try
        //    {
        //        string SenderEmail = configuration["SenderEmail"].ToString();
        //        string SenderPassword = configuration["SenderPassword"].ToString( );
        //        SmtpClient smtpClient = new SmtpClient()
        //        {
        //            Host = configuration["SenderHost"].ToString( ),
        //            UseDefaultCredentials = false,
        //            Port = int.Parse(configuration["SenderPort"]),
        //            Credentials = new System.Net.NetworkCredential(SenderEmail, SenderPassword),
        //            EnableSsl = true,
        //            DeliveryMethod = SmtpDeliveryMethod.Network
        //        };

        //        var mail = new MailMessage
        //        {
        //            From = new MailAddress(SenderEmail)
        //        };

        //        foreach (var email in receiversEmails)
        //        {
        //            mail.To.Add(email);
        //        }

        //        //if (ccEmails != null)
        //        //{
        //        //    foreach (var email in ccEmails)
        //        //    {
        //        //        mail.CC.Add(email);
        //        //    }
        //        //}
        //        string emailBody = null;

        //        emailBody = BuildEmailBody(body, alternateView);
        //        mail.Subject = subject;
        //         mail.Body = emailBody;
        //        mail.IsBodyHtml = _isBodyHtml;
        //        if (alternateView != null)
        //        {
        //            mail.AlternateViews.Add(alternateView);
        //        }
        //        if (logo != null)
        //        {
        //            mail.AlternateViews.Add(logo);
        //        }
        //        smtpClient.Send(mail);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //public    string BuildEmailBody(string message, AlternateView alternateView)
        //{
        //    #region Inject IConfiguration 
        //    IConfiguration configuration = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json")
        //                .AddJsonFile("appsettings.Development.json", optional: true)
        //                .Build();

        //    #endregion
        //    string directoryOfImage = "wwwroot\\Content\\img\\gigJordan.jpg";
        //    var windowsLogo = new LinkedResource(directoryOfImage, Image.Jpeg) { ContentId = "WinLogo" };
        //    var body = @"<div style = 'font-style: normal; font-family:Calibri; font-size: 12pt;' >" + message + "<br/> " + configuration["Regards"] + " < br/><br/>" + configuration["EmilFooter"] + " < hr style='background-color:blue'><img src='cid:WinLogo'/></div>";

        //    alternateView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, Text.Html);
        //    alternateView.LinkedResources.Add(windowsLogo);

        //    return body;
        //}
        public bool SendEmail(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                string fromEmail = emailSettings["FromEmail"];
                string smtpHost = emailSettings["SmtpHost"];
                int smtpPort = int.Parse(emailSettings["SmtpPort"]);
                string smtpUser = emailSettings["SmtpUser"];
                string smtpPassword = emailSettings["SmtpPassword"];

                var mail = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                mail.To.Add(toEmail);

                var smtp = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

}
