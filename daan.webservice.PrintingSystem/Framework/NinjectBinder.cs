using daan.webservice.PrintingSystem.AuthenticaitionImpl;
using daan.webservice.PrintingSystem.Framework.Authenticaition;
using Ninject;

namespace daan.webservice.PrintingSystem.Framework
{
    public static class NinjectBinder
    {
        private static IKernel _ninjectKernel;

        public static void Initialize()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IAuthenticaitionService>().To<CenterAuthenticaitionServiceImpl>();
        }

        public static TInterface Get<TInterface>()
        {
            return _ninjectKernel.Get<TInterface>();
        }
    }
}