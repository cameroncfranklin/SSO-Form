using Microsoft.Extensions.Configuration;
using SSORequestApplication.Models;
using System.Net.Mail;
using System.Text;

namespace SSORequestApplication.HelperClasses
{
    public class EmailHelper
    {

        private string _host;
        private string _from;
        private string _alias;
        public EmailHelper(IConfiguration iConfiguration)
        {
            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
                _alias = smtpSection.GetSection("Alias").Value;
            }
        }

        internal void SendEmail(EmailModel emailModel)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_host))
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_from, _alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);
                    if (emailModel.CC != null)
                    {
                        mailMessage.CC.Add(emailModel.CC);
                    }
                    mailMessage.Body = emailModel.Message;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;
                    client.Send(mailMessage);
                    mailMessage.Dispose();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
