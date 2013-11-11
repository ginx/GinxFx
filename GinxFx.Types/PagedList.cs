// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagedList.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the PagedList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Types
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [Serializable]
    [DataContract]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList()
        {
        }

        public PagedList(int totalCount)
        {
            this.RowCount = totalCount;
        }

        public PagedList(int totalCount, IEnumerable<T> collection) : base(collection)
        {
            this.RowCount = totalCount;
        }

        [DataMember]
        public int RowCount { get; private set; }
    }
}