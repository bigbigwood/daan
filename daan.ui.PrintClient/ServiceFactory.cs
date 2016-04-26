using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.webservice.phyReportSystem.Contract.Interface;
using System.ServiceModel;

namespace daan.ui.PrinterApplication
{
    public static class ServiceFactory
    {
        private static ChannelFactory<IUserService> _userServiceFactory;
        private static ChannelFactory<IPhysicalReportService> _physicalReportServiceFactory;

        public static IUserService GetUserService(String url)
        {
            if (_userServiceFactory == null)
            {
                var endpointAddress = new EndpointAddress(url);
                _userServiceFactory = new ChannelFactory<IUserService>(new BasicHttpBinding(), endpointAddress);
            }

            return _userServiceFactory.CreateChannel();
        }

        public static IPhysicalReportService GetPhysicalReportService(String url)
        {
            if (_physicalReportServiceFactory == null)
            {
                var endpointAddress = new EndpointAddress(url);
                _physicalReportServiceFactory = new ChannelFactory<IPhysicalReportService>(new BasicHttpBinding(), endpointAddress);
            }

            return _physicalReportServiceFactory.CreateChannel();
        }
    }
}
