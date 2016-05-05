using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Interface;

namespace daan.ui.PrinterApplication
{
    public static class ServiceFactory
    {
        private static ChannelFactory<IPrintingServiceContract> _printingServiceFactory;

        public static IPrintingServiceContract GetPrintingService(String url)
        {
            if (_printingServiceFactory == null)
            {
                var endpointAddress = new EndpointAddress(url);
                _printingServiceFactory = new ChannelFactory<IPrintingServiceContract>(new BasicHttpBinding(), endpointAddress);
            }

            return _printingServiceFactory.CreateChannel();
        }
    }
}
