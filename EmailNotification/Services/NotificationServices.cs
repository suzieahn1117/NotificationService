using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailNotification.Models;
using EmailNotification.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EmailNotification.Services
{
    public class NotificationServices
    {
        public interface INotificationService
        {
            Task<NotificationResponse> SendEmailNotification(Email emailInfo);
            NotificationResponse SendSMSNotification(Email emailInfo);
        }

        public class NotificationService : INotificationService
        {
            private readonly INotificationRepository _repo;
   
            public NotificationService(INotificationRepository repo)
            {    
                _repo = repo;

            }

            public async Task<NotificationResponse> SendEmailNotification(Email _emailInfo)
            {
                return await _repo.SendEmailNotification(_emailInfo);
            }

            public NotificationResponse SendSMSNotification(Email _emailInfo)
            {
                return _repo.SendSMSNotification(_emailInfo);
            }
        }

     

    }
}
