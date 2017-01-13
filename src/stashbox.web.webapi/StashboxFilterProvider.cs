using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Stashbox.Infrastructure;
using Stashbox.Utils;

namespace Stashbox.Web.WebApi
{
    /// <summary>
    /// Represents the stashbox filter attribute filter provider.
    /// </summary>
    public class StashboxFilterProvider : IFilterProvider
    {
        private readonly IStashboxContainer stashboxContainer;
        private readonly IEnumerable<IFilterProvider> filterProviders;

        /// <summary>
        /// Constructs a <see cref="StashboxFilterProvider"/>
        /// </summary>
        /// <param name="stashboxContainer">The stashbox container instance.</param>
        /// <param name="filterProviders">The collection of the existing filter providers.</param>
        public StashboxFilterProvider(IStashboxContainer stashboxContainer, IEnumerable<IFilterProvider> filterProviders)
        {
            Shield.EnsureNotNull(stashboxContainer, nameof(stashboxContainer));
            Shield.EnsureNotNull(filterProviders, nameof(filterProviders));

            this.stashboxContainer = stashboxContainer;
            this.filterProviders = filterProviders;
        }

        /// <inheritdoc />
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = this.filterProviders.SelectMany(provider => provider.GetFilters(configuration, actionDescriptor)).ToArray();
            foreach (var filter in filters)
                this.stashboxContainer.BuildUp(filter.Instance);

            return filters;
        }
    }
}
