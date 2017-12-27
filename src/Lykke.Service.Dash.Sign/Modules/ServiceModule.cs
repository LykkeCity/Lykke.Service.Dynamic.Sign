using Autofac;
using Common.Log;
using Lykke.Service.Dash.Sign.Core.Services;
using Lykke.Service.Dash.Sign.Core.Settings.ServiceSettings;
using Lykke.Service.Dash.Sign.Services;
using Lykke.SettingsReader;

namespace Lykke.Service.Dash.Sign.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<DashSignSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<DashSignSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<DashService>()
                .As<IDashService>()
                .SingleInstance()
                .WithParameter("network", _settings.CurrentValue.Network);
        }
    }
}
