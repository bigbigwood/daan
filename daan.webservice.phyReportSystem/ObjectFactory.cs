using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using daan.webservice.phyReportSystem.Framework.Authenticaition;
using daan.webservice.phyReportSystem.AuthenticaitionImpl;

namespace daan.webservice.phyReportSystem
{
    public static class ObjectFactory
    {
        private static IKernel _ninjectKernel;

        public static void Initialize()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IAuthenticaitionService>().To<CenterAuthenticaitionServiceImpl>();
        }

        public static TInterface GetImpl<TInterface>()
        {
            return _ninjectKernel.Get<TInterface>();
        }
    }
}