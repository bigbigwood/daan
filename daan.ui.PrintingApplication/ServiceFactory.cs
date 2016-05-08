using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using daan.webservice.PrintingSystem.Contract.Interface;

namespace daan.ui.PrintingApplication
{
    public static class ServiceFactory
    {
        private static ChannelFactory<IClientApplicationServiceContract> _clientApplicationServiceFactory;
        private static ChannelFactory<IPrintingServiceContract> _printingServiceFactory;

        public static IClientApplicationServiceContract GetClientApplicationService(String url)
        {
            if (_clientApplicationServiceFactory == null)
            {
                var endpointAddress = new EndpointAddress(url);
                _clientApplicationServiceFactory = new ChannelFactory<IClientApplicationServiceContract>(GetBinding(), endpointAddress);
            }

            return _clientApplicationServiceFactory.CreateChannel();
        }

        public static IPrintingServiceContract GetPrintingService(String url)
        {
            if (_printingServiceFactory == null)
            {
                var endpointAddress = new EndpointAddress(url);
                _printingServiceFactory = new ChannelFactory<IPrintingServiceContract>(GetBinding(), endpointAddress);
            }

            return _printingServiceFactory.CreateChannel();
        }

        private static Binding GetBinding()
        {
            return new BasicHttpBinding() { MaxReceivedMessageSize = int.MaxValue };
        }
    }
}
