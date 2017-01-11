using System.Linq;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using Stashbox;
using Stashbox.Entity;
using Stashbox.Infrastructure;
using Stashbox.Web.WebApi;

namespace System.Web.Http
{
    /// <summary>
    /// Represents the <see cref="HttpConfiguration"/> extension for using <see cref="StashboxContainer"/>.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Configures the <see cref="StashboxContainer"/> as the default dependency resolver and sets custom <see cref="IFilterProvider"/> and <see cref="ModelValidatorProvider"/>.
        /// </summary>
        public static void UseStashbox(this HttpConfiguration config, Action<IStashboxContainer> configureAction)
        {
            StashboxConfig.Container.RegisterInstance<IStashboxContainer>(StashboxConfig.Container);
            StashboxConfig.Container.RegisterType<ModelValidatorProvider, StashboxDataAnnotationsModelValidatorProvider>();
            StashboxConfig.Container.PrepareType<ModelValidatorProvider, StashboxModelValidatorProvider>()
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "modelValidatorProviders",
                    Value = config.Services.GetServices(typeof(ModelValidatorProvider))
                                           .Where(provider => !(provider is DataAnnotationsModelValidatorProvider))
                                           .Cast<ModelValidatorProvider>()
                }).Register();

            StashboxConfig.Container.PrepareType<IFilterProvider, StashboxFilterProvider>()
                .WithInjectionParameters(new InjectionParameter
                {
                    Name = "filterProviders",
                    Value = config.Services.GetServices(typeof(IFilterProvider))
                                           .Cast<IFilterProvider>()
                }).Register();

            config.Services.Clear(typeof(IFilterProvider));
            config.Services.Clear(typeof(ModelValidatorProvider));

            config.DependencyResolver = new StashboxDependencyResolver(StashboxConfig.Container);

            configureAction(StashboxConfig.Container);
        }
    }
}
