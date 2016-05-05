using daan.webservice.PrintingSystem.AuthenticaitionImpl;
using daan.webservice.PrintingSystem.Framework.Authenticaition;
using Ninject;

namespace daan.webservice.PrintingSystem
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