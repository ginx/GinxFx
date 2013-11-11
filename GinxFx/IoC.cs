// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the IoC type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx
{
    using System;

    using Munq;

    /// <summary>
    /// The IoC singleton class.
    /// </summary>
    public class IoC
    {
        /// <summary>
        /// The Lazy instance Munq IoC container.
        /// </summary>
        private static readonly Lazy<IocContainer> LazyContainer = new Lazy<IocContainer>(true);

        /// <summary>
        /// Prevents a default instance of the <see cref="IoC"/> class from being created.
        /// </summary>
        private IoC()
        {
        }

        /// <summary>
        /// Gets the Munq IoC container.
        /// </summary>
        /// <value>
        /// The Munq IoC container.
        /// </value>
        public static IocContainer Container
        {
            get
            {
                return LazyContainer.Value;
            }
        }
    }
}
