using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using EasySec.Hashing;
using NBlog.Configuration;
using NBlog.Domain.Builders;
using NBlog.Domain.Entities;
using NBlog.Domain.Mongo.Repositories;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Domain.Mongo;
using NBlog.Infrastructure;
using Ninject.Activation;

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

            // Repositores
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IPostRepository>().To<PostRepository>().InRequestScope();

            // Builders
            kernel.Bind<IBuild<Post>>().To<PostBuilder>().InRequestScope();

            kernel.Bind<IAuthenticationHandler>().To<FormsAuthenticationHandler>().InSingletonScope();
            kernel.Bind<IHashGenerator>().To<HashGenerator>().InSingletonScope();
        }        
    }
}
