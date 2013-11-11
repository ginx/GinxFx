// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPagedList.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the IPagedList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Types
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public interface IPagedList<T> : IList<T>
    {
        int RowCount { get; }
    }
}
