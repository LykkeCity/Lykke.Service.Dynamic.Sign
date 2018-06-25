using Lykke.Service.Dynamic.Sign.Core.Settings.ServiceSettings;
using Lykke.Service.Dynamic.Sign.Core.Settings.SlackNotifications;

namespace Lykke.Service.Dynamic.Sign.Core.Settings
{
    public class AppSettings
    {
        public DynamicSignSettings DynamicSignService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
