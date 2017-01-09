using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ronin.Common;
using Stashbox.Infrastructure;

namespace Stashbox.Web.WebApi
{
    /// <summary>
    /// Represents the stashbox filter attribute filter provider.
    /// </summary>
    public class StashboxFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        private readonly IStashboxContainer stashboxContainer;

        /// <summary>
        /// Constructs a <see cref="StashboxFilterProvider"/>
        /// </summary>
        /// <param name="stashboxContainer">The stashbox container instance.</param>
        public StashboxFilterProvider(IStashboxContainer stashboxContainer)
        {
            Shield.EnsureNotNull(stashboxContainer, nameof(stashboxContainer));

            this.stashboxContainer = stashboxContainer;
        }

        /// <inheritdoc />
        public new IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(configuration, actionDescriptor);
            var filterInfos = filters as FilterInfo[] ?? filters.ToArray();
            foreach (var filter in filterInfos)
                this.stashboxContainer.BuildUp(filter.Instance);

            return filterInfos;
        }
    }
}
