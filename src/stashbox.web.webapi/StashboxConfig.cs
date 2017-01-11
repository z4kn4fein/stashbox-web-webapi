using System;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using Stashbox.Infrastructure;

namespace Stashbox.Web.WebApi
{
    /// <summary>
    /// Represents the <see cref="StashboxConfig"/>.
    /// </summary>
    public static class StashboxConfig
    {
        private static readonly Lazy<IStashboxContainer> stashboxContainer = new Lazy<IStashboxContainer>(() => new StashboxContainer());

        /// <summary>
        /// Singleton instance of the <see cref="StashboxContainer"/>.
        /// </summary>
        public static IStashboxContainer Container => stashboxContainer.Value;

        /// <summary>
        /// Configures the <see cref="StashboxContainer"/> as the default dependency resolver and sets custom <see cref="IFilterProvider"/> and <see cref="ModelValidatorProvider"/>.
        /// </summary>
        public static void RegisterStashbox(HttpConfiguration configuration, Action<IStashboxContainer> configureAction)
        {
            configuration.UseStashbox(configureAction);
        }
    }
}
