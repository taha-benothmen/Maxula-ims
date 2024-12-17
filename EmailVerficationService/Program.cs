using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailSenderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string senderEmail = "tahabenothmen10@gmail.com";
            string senderPassword = "ncnt mxsp arvd gjbk"; // Replace with your App Password

            List<string> recipientEmails = new List<string>
            {
                "tahabenothman245@gmail.com",
                "tahabenothmen5@gmail.com"
            };

            string subject = "Your Confirmation Code";

            Dictionary<string, string> confirmationCodes = GenerateConfirmationCodes(recipientEmails);

            await SendEmails(senderEmail, senderPassword, recipientEmails, subject, confirmationCodes);
        }

        static async Task SendEmails(string senderEmail, string senderPassword, List<string> recipientEmails, string subject, Dictionary<string, string> confirmationCodes)
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(senderEmail, senderPassword);

                foreach (var recipientEmail in recipientEmails)
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Your Company", senderEmail));
                    email.To.Add(new MailboxAddress("Recipient", recipientEmail));
                    email.Subject = subject;

                    string confirmationCode = confirmationCodes[recipientEmail];

                    string body = $@"
                    <html>
                        <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4;'>
                            <div style='max-width: 600px; margin: 20px auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); text-align: center;'>
                                <img src='https://via.placeholder.com/100' alt='Thumbs Up' style='width: 100px; margin-bottom: 20px;' />
                                <h2 style='color: #333;'>Hi!</h2>
                                <p style='font-size: 16px; color: #555;'>Here is the confirmation code :</p>
                                <h1 style='font-size: 36px; color: #f44336; margin: 20px 0;'>{confirmationCode}</h1>
                                <p style='font-size: 16px; color: #555;'>All you have to do is copy the confirmation code and paste it into your form to complete the email verification process.</p>
                                <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;' />
                                <p style='font-size: 12px; color: #999;'>If you didn’t request this, please ignore this email.</p>
                            </div>
                        </body>
                    </html>";

                    email.Body = new TextPart("html")
                    {
                        Text = body
                    };

                    try
                    {
                        await smtp.SendAsync(email);
                        Console.WriteLine($"Email sent to {recipientEmail}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email to {recipientEmail}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect or authenticate: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        static Dictionary<string, string> GenerateConfirmationCodes(List<string> emails)
        {
            var codes = new Dictionary<string, string>();
            foreach (var email in emails)
            {
                string code = new Random().Next(1000, 9999).ToString();
                codes[email] = code;
            }
            return codes;
        }
    }
}
