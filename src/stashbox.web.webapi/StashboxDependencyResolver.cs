using Stashbox.Infrastructure;
using Stashbox.Utils;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Stashbox.Web.WebApi
{
    /// <summary>
    /// Represents the stashbox dependency resolver.
    /// </summary>
    public class StashboxDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IStashboxContainer stashboxContainer;

        /// <summary>
        /// Constructs a <see cref="StashboxDependencyResolver"/>
        /// </summary>
        /// <param name="stashboxContainer">The stashbox container instance.</param>
        public StashboxDependencyResolver(IStashboxContainer stashboxContainer)
        {
            Shield.EnsureNotNull(stashboxContainer, nameof(stashboxContainer));

            this.stashboxContainer = stashboxContainer;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            return this.stashboxContainer.IsRegistered(serviceType) ? this.stashboxContainer.Resolve(serviceType) : null;
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.stashboxContainer.IsRegistered(serviceType) ? this.stashboxContainer.ResolveAll(serviceType) : new List<object>();
        }

        /// <inheritdoc />
        public IDependencyScope BeginScope()
        {
            return new StashboxDependencyResolver(this.stashboxContainer.BeginScope());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.stashboxContainer.Dispose();
        }
    }
}
