using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using OCHPlanner3.Services.Email.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Email
{
    public class EmailSender : IEmailSender
    {
		private readonly EmailSettings _emailSettings;

		public EmailSender(
			IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			try
			{
				//Before sending anything, check if DebugEmail is present
				if (!String.IsNullOrEmpty(_emailSettings.DebugEmail))
					email = _emailSettings.DebugEmail;

				var mimeMessage = new MimeMessage();

				mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

				mimeMessage.To.Add(new MailboxAddress(email));

				mimeMessage.Subject = subject;

				mimeMessage.Body = new TextPart("html")
				{
					Text = message
				};

				var cts = new CancellationTokenSource();
				var token = cts.Token;
				cts.CancelAfter(4000);

				using (var client = new SmtpClient())
				{
					// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
					//client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					// The third parameter is useSSL (true if the client should make an SSL-wrapped
					// connection to the server; otherwise, false).
					await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);

					// Note: only needed if the SMTP server requires authentication
					await client.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);

					await client.SendAsync(mimeMessage);

					await client.DisconnectAsync(true);
				}

			}
			catch (Exception ex)
			{
				// TODO: handle exception
				throw new InvalidOperationException(ex.Message);
			}
		}
	}
}
