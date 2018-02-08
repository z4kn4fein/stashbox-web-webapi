using Stashbox.Entity;
using Stashbox.Lifetime;
using Stashbox.Utils;
using Stashbox.Web.WebApi;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;

namespace Stashbox
{
    /// <summary>
    /// Represents the web api related extensions of <see cref="IStashboxContainer"/>.
    /// </summary>
    public static class StashboxContainerExtensions
    {
        /// <summary>
        /// Configures the <see cref="IStashboxContainer"/> as the default dependency resolver and sets custom <see cref="IFilterProvider"/> and <see cref="ModelValidatorProvider"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="container"/> or <paramref name="config"/> is <c>null</c>.
        /// </exception>
        public static IStashboxContainer AddWebApi(this IStashboxContainer container, HttpConfiguration config)
        {
            Shield.EnsureNotNull(container, nameof(container));
            Shield.EnsureNotNull(config, nameof(config));

            container.AddWebApiModelValidatorInjection(config);
            container.AddWebApiFilterProviderInjection(config);

            config.DependencyResolver = new StashboxDependencyResolver(container);
            container.RegisterWebApiControllers(config);

            return container;
        }

        /// <summary>
        /// Sets a custom <see cref="ModelValidatorProvider"/> which enables property injection in model validators.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="container"/> or <paramref name="config"/> is <c>null</c>.
        /// </exception>
        public static IStashboxContainer AddWebApiModelValidatorInjection(this IStashboxContainer container, HttpConfiguration config)
        {
            Shield.EnsureNotNull(container, nameof(container));
            Shield.EnsureNotNull(config, nameof(config));

            container.RegisterType<ModelValidatorProvider, StashboxDataAnnotationsModelValidatorProvider>();
            container.RegisterType<ModelValidatorProvider, StashboxModelValidatorProvider>(context => context
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "modelValidatorProviders",
                    Value = config.Services.GetServices(typeof(ModelValidatorProvider))
                                           .Where(provider => !(provider is DataAnnotationsModelValidatorProvider))
                                           .Cast<ModelValidatorProvider>()
                }));

            config.Services.Clear(typeof(ModelValidatorProvider));

            return container;
        }

        /// <summary>
        /// Sets a custom <see cref="IFilterProvider"/> which enables property injection in filters.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="container"/> or <paramref name="config"/> is <c>null</c>.
        /// </exception>
        public static IStashboxContainer AddWebApiFilterProviderInjection(this IStashboxContainer container, HttpConfiguration config)
        {
            Shield.EnsureNotNull(container, nameof(container));
            Shield.EnsureNotNull(config, nameof(config));

            container.RegisterType<IFilterProvider, StashboxFilterProvider>(context => context
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "filterProviders",
                    Value = config.Services.GetServices(typeof(IFilterProvider))
                                           .Cast<IFilterProvider>()
                }));

            config.Services.Clear(typeof(IFilterProvider));

            return container;
        }

        /// <summary>
        /// Registers the web api controllers into the <see cref="IStashboxContainer"/>.
        /// </summary>
        /// <param name="config">The http configuration.</param>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="container"/> or <paramref name="config"/> is <c>null</c>.
        /// </exception>
        public static IStashboxContainer RegisterWebApiControllers(this IStashboxContainer container, HttpConfiguration config)
        {
            Shield.EnsureNotNull(container, nameof(container));
            Shield.EnsureNotNull(config, nameof(config));

            var assembliesResolver = config.Services.GetAssembliesResolver();
            var typeResolver = config.Services.GetHttpControllerTypeResolver();
            container.RegisterTypes(typeResolver.GetControllerTypes(assembliesResolver), null,
                context => context.WithLifetime(new ScopedLifetime()));

            return container;
        }
    }
}
