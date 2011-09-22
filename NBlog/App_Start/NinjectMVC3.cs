using System.Web.Security;
using EasySec.Hashing;
using NBlog.Configuration;
using NBlog.Data;
using NBlog.Data.Mongo;
using NBlog.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NBlog.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NBlog.App_Start.NinjectMVC3), "Stop")]

namespace NBlog.App_Start
{
    using System.Reflection;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<INBlogConfiguration>().To<NBlogConfiguration>().InSingletonScope();
            kernel.Bind<IMongoConfiguration>().To<NBlogMongoConfiguration>().InSingletonScope();
            kernel.Bind<IUserRepository>().To<MongoUserRepository>().InRequestScope();
            kernel.Bind<MembershipProvider>().To<NBlogMembershipProvider>();
            kernel.Bind<IAuthenticationManager>().To<FormsAuthenticationManager>().InSingletonScope();
            kernel.Bind<IHashGenerator>().To<HashGenerator>().InSingletonScope();
        }        
    }
}
