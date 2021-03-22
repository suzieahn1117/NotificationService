using EmailNotification.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EmailNotification.Repository
{
    public interface INotificationRepository
    {
        Task<NotificationResponse> SendEmailNotification(Email emailInfo);
        NotificationResponse SendSMSNotification(Email emailInfo);
    }

    public class NotificationRepository : INotificationRepository
    {
        private SendGridClient _client;
        private IConfiguration _config;

        public NotificationRepository(IConfiguration config)
        {
            
            _config = config;
            var apiKey = _config["Settings:SendGridApiKey"];
            _client = new SendGridClient(apiKey);
        }

        public async Task<NotificationResponse> SendEmailNotification(Email emailInfo)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("suziesujinahn@gmail.com", "Suzie Ahn"),
                    Subject = emailInfo.Subject,
                    PlainTextContent = emailInfo.PlainTextContent
                };

                msg.AddTo(new EmailAddress(emailInfo.To, "Suzie Ahn"));
                var response = await _client.SendEmailAsync(msg).ConfigureAwait(false);

                NotificationResponse notificationResult = new NotificationResponse
                {
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode

                };

                return notificationResult;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new NotificationResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    Message = "Failed to send email"
                };
            }
        }

        public NotificationResponse SendSMSNotification(Email userInput)
        {
            try
            {
                var accountSid = _config["Settings:TwilioApiKey"]; 
                var authToken = _config["Settings:TwilioAuthToken"];
                TwilioClient.Init(accountSid, authToken);
                var fromNumber = new Twilio.Types.PhoneNumber("+14243484278");
                var toNumber = new Twilio.Types.PhoneNumber("+1" + userInput.Phone);
                var msg = MessageResource.Create(
                        body: "Hello! Email has arrived!",
                        from: fromNumber,
                        to: toNumber
                    );

                NotificationResponse response = new NotificationResponse();

                if(msg.ErrorCode != null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.IsSuccessStatusCode = false;
                    response.Message = msg.ErrorMessage;
                    return response;
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.IsSuccessStatusCode = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new NotificationResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    Message ="Failed to send SMS"
                   
                };

            }

        }
    }
}
