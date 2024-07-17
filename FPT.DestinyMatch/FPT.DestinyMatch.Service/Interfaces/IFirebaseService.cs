using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IFirebaseService
    {
        Task SendNotification(string fcmToken, string? title, string body);
    }
}
