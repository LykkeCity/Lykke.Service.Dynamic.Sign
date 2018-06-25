using Autofac;
using Common.Log;
using Lykke.Service.Dynamic.Sign.Core.Services;
using Lykke.Service.Dynamic.Sign.Core.Settings.ServiceSettings;
using Lykke.Service.Dynamic.Sign.Services;
using Lykke.SettingsReader;

namespace Lykke.Service.Dynamic.Sign.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<DynamicSignSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<DynamicSignSettings> settings, ILog log)
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

            builder.RegisterType<DynamicService>()
                .As<IDynamicService>()
                .SingleInstance()
                .WithParameter("network", _settings.CurrentValue.Network);
        }
    }
}
