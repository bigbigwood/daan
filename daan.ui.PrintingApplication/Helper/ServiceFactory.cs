﻿using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using daan.webservice.PrintingSystem.Contract.Interface;

namespace daan.ui.PrintingApplication.Helper
{
    public static class ServiceFactory
    {
        private static ChannelFactory<IClientApplicationServiceContract> _clientApplicationServiceFactory;
        private static ChannelFactory<IPrintingServiceContract> _printingServiceFactory;

        public static IClientApplicationServiceContract GetClientApplicationService()
        {
            if (_clientApplicationServiceFactory == null)
            {
                string url = ConfigurationManager.AppSettings.Get("ClientApplicationServiceUrl");
                var endpointAddress = new EndpointAddress(url);
                _clientApplicationServiceFactory = new ChannelFactory<IClientApplicationServiceContract>(GetBinding(), endpointAddress);
            }

            return _clientApplicationServiceFactory.CreateChannel();
        }

        public static IPrintingServiceContract GetPrintingService()
        {
            if (_printingServiceFactory == null)
            {
                string url = ConfigurationManager.AppSettings.Get("PrintingServiceServiceUrl");
                var endpointAddress = new EndpointAddress(url);
                _printingServiceFactory = new ChannelFactory<IPrintingServiceContract>(GetBinding(), endpointAddress);
            }

            return _printingServiceFactory.CreateChannel();
        }

        private static Binding GetBinding()
        {
            TimeSpan timeout = new TimeSpan(0,0,10,0);
            return new BasicHttpBinding()
            {
                SendTimeout = timeout,
                ReceiveTimeout = timeout, 
                MaxReceivedMessageSize = int.MaxValue, 
            };
        }
    }
}
