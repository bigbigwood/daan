using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using daan.webservice.PrintingSystem.Repository.Interfaces;
using daan.webservice.PrintingSystem.Repository.MyBatis;
using daan.webservice.PrintingSystem.Repository.MyBatis.Impl;
using Ninject;

namespace daan.webservice.PrintingSystem.Repository
{
    public static class NinjectBinder
    {
        private static IKernel _ninjectKernel;

        public static void Initialize()
        {
            _ninjectKernel = new StandardKernel();

            _ninjectKernel.Bind<IPersistanceConnection>().To<SessionToPersistanceAdapter>();
            _ninjectKernel.Bind<IConnectionProvider>().To<MyBatisConnectionProvider>();
            _ninjectKernel.Bind<IPersistanceTransaction>().To<TransactionToPersistanceTransaction>();

            _ninjectKernel.Bind<IOrderRepository>().To<OrderRepositoryMB>();
            _ninjectKernel.Bind<IDictUserRepository>().To<DictUserRepositoryMB>();
            _ninjectKernel.Bind<IOperationLogRepository>().To<OperationLogRepositoryMB>();
            _ninjectKernel.Bind<IInitBasicRepository>().To<InitBasicRepositoryMB>();
            _ninjectKernel.Bind<IOrderReportRepository>().To<OrderReportRepositoryMB>();
        }

        public static TInterface Get<TInterface>()
        {
            return _ninjectKernel.Get<TInterface>();
        }
    }
}