using System.Collections.Generic;
using System.Web.Mvc;
using NBlog.Filters;
using NBlog.Services;
using NBlog.Views;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using TJ.CQRS.Messaging;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NBlog.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NBlog.App_Start.NinjectWebCommon), "Stop")]

namespace NBlog.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new NBlogNinjectModule());
        }        
    }

    internal class NBlogNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlogView>().To<BlogView>();
            Bind<IAuthorView>().To<AuthorView>();
            Bind<IViewRepository<BlogViewItem>>().To<InMemoryViewRepository<BlogViewItem>>();
            Bind<IViewRepository<Author>>().To<InMemoryViewRepository<Author>>();
            Bind<ISendCommand>().To<InMemoryBus>();
            Bind<IMessageRouter>().To<MessageRouter>();

#if DEBUG
            Bind<IAuthenticationService>().To<AuthenticationServiceStub>();
#else
            Bind<IAuthenticationService>().To<AuthenticationService>();
#endif
            this.BindFilter<BlogExistFilter>(FilterScope.Global, 0);
        }
    }
}
