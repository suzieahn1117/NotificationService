using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmailNotification.Models;
using EmailNotification.Services;
using SendGrid;
using static EmailNotification.Services.NotificationServices;

namespace EmailNotification.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotificationService _service;

        public HomeController(INotificationService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {

            ViewBag.Status = "";
            return View();
        }

        /// <summary>
        /// Call this function to send email and sms notification when 'Send Notificaitaon' is clicked 
        /// </summary>
        /// <param name="emailInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SendNotification")]
        public async Task<ActionResult> SendNotification(Email emailInfo)
        {
            try
            {
                //NotificationResponse response = (NotificationResponse)await notif.SendEmailNotification(emailInfo); // Send SendGrid Email Notification

               NotificationResponse response = await _service.SendEmailNotification(emailInfo);

                if (response.IsSuccessStatusCode) //When Email noficiation is successful, send Twilio SMS notification 
                {
                    NotificationResponse smsResponse = _service.SendSMSNotification(emailInfo);

                    if (smsResponse.IsSuccessStatusCode)
                    {
                        ViewBag.Status = "Email and SMS Successfully Sent"; //Set successful message when Twilio SMS is successful
                    }
                    else
                    {
                        ViewBag.Status = "Email successfully sent";
                    }

                }
                else
                {
                    ViewBag.Status = "Notification failed to send"; //Email failed to send. No notification for SMS
                }

                return View("Index");

            }
            catch(Exception ex)
            {
                ViewBag.Status = ex.Message;
                return View("Index");
            }
        }
    }
}
