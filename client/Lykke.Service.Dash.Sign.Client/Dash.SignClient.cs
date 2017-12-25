using System;
using Common.Log;

namespace Lykke.Service.Dash.Sign.Client
{
    public class DashSignClient : IDashSignClient, IDisposable
    {
        private readonly ILog _log;

        public DashSignClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
