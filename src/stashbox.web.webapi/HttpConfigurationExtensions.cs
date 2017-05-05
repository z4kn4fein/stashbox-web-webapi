using System.Web.Http.Filters;
using System.Web.Http.Validation;
using Stashbox;
using Stashbox.Infrastructure;
using Stashbox.Utils;
using Stashbox.Web.WebApi;

namespace System.Web.Http
{
    /// <summary>
    /// Represents the <see cref="HttpConfiguration"/> extension for using <see cref="StashboxContainerExtensions"/>.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Configures the <see cref="StashboxContainerExtensions"/> as the default dependency resolver and sets custom <see cref="IFilterProvider"/> and <see cref="ModelValidatorProvider"/>.
        /// </summary>
        public static void UseStashbox(this HttpConfiguration config, Action<IStashboxContainer> configureAction = null)
        {
            var container = new StashboxContainer(conf => conf
                .WithCircularDependencyTracking()
                .WithDisposableTransientTracking());

            container.AddWebApi(config, configureAction);
        }

        /// <summary>
        /// Configures the <see cref="StashboxContainerExtensions"/> as the default dependency resolver and sets custom <see cref="IFilterProvider"/> and <see cref="ModelValidatorProvider"/>.
        /// </summary>
        public static void UseStashbox(this HttpConfiguration config, IStashboxContainer container, Action<IStashboxContainer> configureAction = null)
        {
            Shield.EnsureNotNull(container, nameof(container));

            container.AddWebApi(config, configureAction);
        }
    }
}
