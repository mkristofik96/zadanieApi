using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace ZadanieCoderama.Helpers
{
    public class EmailClient
    {
        public static void SendMail(string body,string receiver)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mkristofikSMTP@gmail.com", "qxbbjsmuffijfclv"),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mkristofikSMTP@gmail.com"),
                    Subject = "Convert data",
                    Body = $"{body}"

                };
                mailMessage.To.Add(receiver);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
