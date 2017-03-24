using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Stashbox.Web.WebApi
{
    /// <summary>
    /// Represents the stashbox dependency resolver.
    /// </summary>
    public class StashboxDependencyResolver : IDependencyResolver
    {
        private readonly Infrastructure.IDependencyResolver dependencyResolver;

        /// <summary>
        /// Constructs a <see cref="StashboxDependencyResolver"/>
        /// </summary>
        /// <param name="dependencyResolver">The stashbox container instance.</param>
        public StashboxDependencyResolver(Infrastructure.IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType) => this.dependencyResolver.Resolve(serviceType, nullResultAllowed: true);

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType) => this.dependencyResolver.ResolveAll(serviceType);

        /// <inheritdoc />
        public IDependencyScope BeginScope() => new StashboxDependencyResolver(this.dependencyResolver.BeginScope());

        /// <inheritdoc />
        public void Dispose() => this.dependencyResolver.Dispose();
    }
}
