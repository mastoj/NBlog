using System.Web.Http.Filters;
using NBlog.Infrastructure.MessageRouting;
using NBlog.Views;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using TJ.CQRS;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;
using TJ.CQRS.RavenEvent;
using TJ.CQRS.Respositories;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NBlog.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NBlog.Web.App_Start.NinjectWebCommon), "Stop")]

namespace NBlog.Web.App_Start
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
            var connectionStringName = "RavenDB";
            Bind<ICommandRouter>().To<CommandRouter>().InRequestScope();
            Bind<IEventRouter>().To<EventRouter>().InRequestScope();
            Bind<IBlogView>().To<BlogView>().InRequestScope();
            Bind<IUserView>().To<UserView>().InRequestScope();
            Bind<IPostView>().To<PostView>().InRequestScope();
            Bind(typeof(IViewRepository<>)).To(typeof(RavenViewRepository<>)).InRequestScope().WithConstructorArgument("connectionStringName", connectionStringName);
            Bind<IDomainRepositoryFactory>().To<DomainRepositoryFactory>().InRequestScope();
            Bind<IEventBus>().To<InMemoryEventBus>().InRequestScope();
            Bind<IEventStore>().To<RavenEventStore>().InRequestScope().WithConstructorArgument("connectionStringName", connectionStringName);
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            Bind<ICommandBus>().To<InMemoryCommandBus>().InRequestScope();

#if DEBUG
//            Bind<IAuthenticationService>().To<AuthenticationServiceStub>();
#else
            Bind<IAuthenticationService>().To<AuthenticationService>();
#endif
//            this.BindFilter<BlogExistFilter>(FilterScope.Global, 0);
        }
    }
}
