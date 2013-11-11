// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   The Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Data
{
    using System.Collections.Generic;

    using GinxFx.Types;

    /// <summary>
    /// The repository interface to be used to implement data access for entities.
    /// </summary>
    /// <typeparam name="TId">
    /// Defines the type of database primary key.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// Defines the entity which this class will serve.
    /// </typeparam>
    public interface IRepository<in TId, TEntity>
        where TEntity : Entity<TId, TEntity>
    {
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        void Delete(TEntity domainEntity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">
        /// The entity id.
        /// </param>
        void Delete(TId id);

        /// <summary>
        /// Gets all entities from persistent media.
        /// </summary>
        /// <returns>
        /// All entities.
        /// </returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// Gets all entities from persistent media.
        /// </summary>
        /// <param name="page">The actual page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>All entities, in the specified page.</returns>
        IPagedList<TEntity> GetAll(int page, int pageSize);

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">
        /// The entity id.
        /// </param>
        /// <returns>
        /// The entity specified by key <paramref name="id"/> or <c>null</c> if not found.
        /// </returns>
        TEntity GetById(TId id);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        TEntity Save(TEntity domainEntity);

        /// <summary>
        /// Save or update the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        TEntity SaveOrUpdate(TEntity domainEntity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        TEntity Update(TEntity domainEntity);
    }
}