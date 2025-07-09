using System.Net;
using System.Net.Mail;
namespace SignatorDocSolution.Utils
{
    /// <summary>
    /// CLASS_ID=12;
    /// </summary>
    public static class E_Email 
    {
        private static string smtpHost = "smtp.gmail.com";
        private static int portNumber = 587;
        private static bool enableSSL = true;

        private static string emailFrom = "SignatorDoc@email.com";
        private static string password = "abc123456";
        private static string emailTo = "email@email.com";
        private static string subject = "Licensa de Uso SignatorDoc";
        private static string body = "Olá, segue em anexo o pedido de licença do usuário do sistema SignatorDoc.<p/>Remetente: ";
        private static string footer = "<p/><p/>Att,<br/>Equipe SignatorDoc";
        private static string filenameAtach = null;

        public static void ConfigMail(string addressee,string filename)
        {
            body += addressee;
            filenameAtach = filename;
        }

        public static bool sendEmail() {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    body += footer;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    mail.Attachments.Add(new Attachment(filenameAtach));
                    
                    SmtpClient smtp = new SmtpClient(smtpHost, portNumber);
                    smtp.UseDefaultCredentials = false;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                    return true;
                }
            }
            catch (System.Net.Mail.SmtpException smtpex) {
                System.Windows.Forms.MessageBox.Show("Falha no envio do Email. Verifique se existe um Firewall ou Antivírus bloqueando o envio. Erro: " + smtpex.Message, "Email", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (System.Exception ex) {
                System.Windows.Forms.MessageBox.Show("Falha no envio do Email. Verifique se existe um Firewall ou Antivírus bloqueando o envio. Erro: " + ex.Message, "Email", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return false;
            
        }
    }

}