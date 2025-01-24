    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class AdvancedNotificationService : NotificationService
    {
        public AdvancedNotificationService(string serviceName = "Advanced") : base(serviceName)
        {
        }

        public override void SendNotification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty or null");
            }
            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[{timestamp}] Sending advanced notification: {message}");
        }
    }
}
