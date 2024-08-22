using MailKit.Net.Smtp;
using MimeKit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DarkDemo.Clases
{
    internal class sendMail
    {

        public void EnviarVariosMail(string data, string topico)
        {
            try
            {
                CConexion cConexion = new CConexion();
                String query = "SELECT mail FROM users";
                DateTime fecha = DateTime.Now;
                string asunto = "Alerta por Niveles de oxígeno";
                string cuerpo = "Datos de niveles de " + topico + " muy bajos. Valor: " + data + ", en la fecha: " + fecha;

                using (MySqlConnection connection = cConexion.establecerConexion())
                {
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string destino = reader["mail"].ToString();
                            mailSender(destino, asunto, cuerpo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al enviar el email: " + ex.Message);
            }
        }

        internal void SendMail(System.Windows.Forms.TextBox destino, System.Windows.Forms.TextBox asunto, System.Windows.Forms.TextBox cuerpo)
        {
            string loadDestino = destino.Text;
            string loadAsunto = asunto.Text;
            string loadCuerpo = cuerpo.Text;

            mailSender(loadDestino, loadAsunto, loadCuerpo);
        }

        public void mailSender(string destino, string asunto, string cuerpo, byte[] pdfBites = null, string pdfFileName = null)
        {
            try
            {
                string servidor = "smtp.gmail.com";
                int puerto = 587;

                string gmailUser = "pae73760@gmail.com";
                string gmailPassword = "mbds laoq ineb swln";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Pisicultura", gmailUser));
                message.To.Add(new MailboxAddress("Destino", destino));
                message.Subject = asunto;

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = cuerpo
                };

                if (pdfBites != null && pdfBites.Length > 0 && !string.IsNullOrEmpty(pdfFileName))
                {
                    // Crear un MimePart usando un byte array en lugar de un MemoryStream
                    var attachment = new MimePart("application", "pdf")
                    {
                        Content = new MimeContent(new MemoryStream(pdfBites)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        FileName = pdfFileName
                    };
                    bodyBuilder.Attachments.Add(attachment);
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.CheckCertificateRevocation = false;
                    smtpClient.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(gmailUser, gmailPassword);
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
