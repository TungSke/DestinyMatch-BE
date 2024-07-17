using FirebaseAdmin.Messaging;
using FPT.DestinyMatch.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Services
{
    public class FirebaseService : IFirebaseService
    {
        public async Task SendNotification(string fcmToken, string? title, string body)
        {
            var message = new Message()
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };
            try
            {
                Console.WriteLine("Sending notification to: " + fcmToken);
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}
