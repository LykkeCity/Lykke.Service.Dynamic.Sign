using System.Threading.Tasks;

namespace Lykke.Service.Dash.Sign.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}