using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using Stashbox.Entity;
using Stashbox.Lifetime;
using Stashbox.Web.WebApi;

namespace Stashbox.Infrastructure
{
    /// <summary>
    /// Represents the web api related extensions of <see cref="IStashboxContainer"/>.
    /// </summary>
    public static class StashboxContainerExtensions
    {
        internal static IStashboxContainer AddWebApi(this IStashboxContainer container, HttpConfiguration config, Action<IStashboxContainer> configureAction = null)
        {
            container.RegisterInstance(container);
            container.RegisterType<ModelValidatorProvider, StashboxDataAnnotationsModelValidatorProvider>();
            container.RegisterType<ModelValidatorProvider, StashboxModelValidatorProvider>(context => context
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "modelValidatorProviders",
                    Value = config.Services.GetServices(typeof(ModelValidatorProvider))
                                           .Where(provider => !(provider is DataAnnotationsModelValidatorProvider))
                                           .Cast<ModelValidatorProvider>()
                }));

            container.RegisterType<IFilterProvider, StashboxFilterProvider>(context => context
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "filterProviders",
                    Value = config.Services.GetServices(typeof(IFilterProvider))
                                           .Cast<IFilterProvider>()
                }));

            config.Services.Clear(typeof(IFilterProvider));
            config.Services.Clear(typeof(ModelValidatorProvider));

            config.DependencyResolver = new StashboxDependencyResolver(container);
            container.RegisterWebApiControllers(config);
            configureAction?.Invoke(container);

            return container;
        }

        /// <summary>
        /// Registers the web api controllers into the <see cref="IStashboxContainer"/>.
        /// </summary>
        /// <param name="config">The http configuration.</param>
        /// <param name="container">The container.</param>
        public static IStashboxContainer RegisterWebApiControllers(this IStashboxContainer container, HttpConfiguration config)
        {
            var assembliesResolver = config.Services.GetAssembliesResolver();
            var typeResolver = config.Services.GetHttpControllerTypeResolver();
            container.RegisterTypes(typeResolver.GetControllerTypes(assembliesResolver), null,
                context => context.WithLifetime(new ScopedLifetime()));

            return container;
        }
    }
}
