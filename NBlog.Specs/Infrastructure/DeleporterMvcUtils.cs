using System;
using System.Web.Mvc;
using DeleporterCore.Client;
using NBlog.Domain.Repositories;
using NBlog.Tests.Areas.Admin.Controllers;
using Ninject;
using Ninject.Web.Mvc;

namespace NBlog.Specs.Infrastructure
{
    public static class DeleporterMvcUtils
    {
        public static void TemporarilyReplaceBinding<TService>(TService implementation)
        {
            var dependencyResolver = ((NinjectDependencyResolver)DependencyResolver.Current);
            var kernel = dependencyResolver.GetService(typeof(IKernel)) as IKernel;
            var oldBindings = kernel.GetBindings(typeof(TService));
            kernel.Rebind<TService>().ToConstant(implementation);
            TidyupUtils.AddTidyupTask(() =>
            {
                var bindings = kernel.GetBindings(typeof(TService));
                foreach (var binding in bindings)
                {
                    kernel.RemoveBinding(binding);
                }
                foreach (var oldBinding in oldBindings)
                {
                    kernel.AddBinding(oldBinding);
                }
            });
        }
    }
}
