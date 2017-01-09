using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Stashbox.Infrastructure;

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
            this.stashboxContainer = stashboxContainer;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            return this.stashboxContainer.Resolve(serviceType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.stashboxContainer.ResolveAll(serviceType);
        }

        /// <inheritdoc />
        public IDependencyScope BeginScope()
        {
            return new StashboxDependencyResolver(this.stashboxContainer.CreateChildContainer());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.stashboxContainer.Dispose();
        }
    }
}
