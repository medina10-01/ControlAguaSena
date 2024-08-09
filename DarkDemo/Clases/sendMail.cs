using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DarkDemo.Clases
{
    internal class sendMail
    {
        internal void SendMail(System.Windows.Forms.TextBox destino, System.Windows.Forms.TextBox asunto, System.Windows.Forms.TextBox cuerpo)
        {
            try
            {
                String Servidor = "smtp.gmail.com";
                int Puerto = 587;

                String GmailUser = "pae73760@gmail.com";
                String GmailPassword = "mbds laoq ineb swln"; // Asegúrate de tener la contraseña aquí

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Pae", GmailUser));
                message.To.Add(new MailboxAddress("Destino", destino.Text));
                message.Subject = asunto.Text;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = cuerpo.Text;

                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.CheckCertificateRevocation = false;
                    smtpClient.Connect(Servidor, Puerto, MailKit.Security.SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(GmailUser, GmailPassword);
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);
                }

                MessageBox.Show("Correo enviado exitosamente.");
            }
            catch (SmtpCommandException ex)
            {
                 MessageBox.Show($"Error en el comando SMTP: {ex.Message}");
                MessageBox.Show($"Código de estado: {ex.StatusCode}");
            }
            catch (SmtpProtocolException ex)
            {
                MessageBox.Show($"Error en el protocolo SMTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}");
            }
        }
    }
}
