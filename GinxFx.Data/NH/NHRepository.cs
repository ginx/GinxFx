// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHRepository.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the NHRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Data.NH
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using GinxFx.Types;

    using NHibernate;

    /// <summary>
    /// The abstract base repository to be used to implement data access for entities.
    /// </summary>
    /// <typeparam name="TId">
    /// Defines the type of database primary key.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// Defines the entity which this class will serve.
    /// </typeparam>
    public abstract class NHRepository<TId, TEntity> : IRepository<TId, TEntity>
        where TEntity : Entity<TId, TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHRepository{TId,TEntity}"/> class. 
        /// Initializes a new instance of the <see cref="NHRepository{TId,TEntity}"/> class.
        /// </summary>
        protected NHRepository()
        {
            this.Session = IoC.Container.Resolve<ISession>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHRepository{TId,TEntity}"/> class. 
        /// Initializes a new instance of the <see cref="NHRepository{TId,TEntity}"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        protected NHRepository(ISession session)
        {
            this.Session = session;
        }

        protected IQueryOver<TEntity, TEntity> QueryOver
        {
            get
            {
                return this.Session.QueryOver<TEntity>();
            }
        } 

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value> The session.</value>
        protected ISession Session { get; private set; }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        public virtual void Delete(TEntity domainEntity)
        {
            using (var transaction = new TransactionRequired(this.Session))
            {
                this.Session.Delete(domainEntity);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">
        /// The entity id.
        /// </param>
        public void Delete(TId id)
        {
            using (var transaction = new TransactionRequired(this.Session))
            {
                this.Delete(this.GetById(id));
                transaction.Commit();
            }
        }

        /// <summary>
        /// Gets all entity from persistent media.
        /// </summary>
        /// <returns>
        /// All the instances of entity.
        /// </returns>
        public virtual IList<TEntity> GetAll()
        {
            return this.GetByCriteria(this.Session.CreateCriteria(typeof(TEntity)));
        }

        /// <summary> Gets all entity from persistent media.
        /// </summary>
        /// <param name="page">The actual page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// All the instances of entity, in the specified page.
        /// </returns>
        public virtual IPagedList<TEntity> GetAll(int page, int pageSize)
        {
            return this.GetByCriteria(this.Session.CreateCriteria(typeof(TEntity)), page, pageSize);
        }

        /// <summary>
        /// Gets the by criteria.
        /// </summary>
        /// <param name="criteria">
        /// The criteria.
        /// </param>
        /// <returns>
        /// All instances of entity matching the specified criteria.
        /// </returns>
        public virtual IList<TEntity> GetByCriteria(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            return criteria.List<TEntity>();
        }

        /// <summary>
        /// Gets the by criteria using pagination.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="page">The actual page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// All instances of entity matching the specified criteria, in the specified page.
        /// </returns>
        public virtual IPagedList<TEntity> GetByCriteria(ICriteria criteria, int page, int pageSize)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("page", "Page must start with 1 and can't be empty.");
            }

            var rowCount = CriteriaTransformer.TransformToRowCount(criteria).UniqueResult<int>();
            var collection = criteria.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).List<TEntity>();

            return new PagedList<TEntity>(rowCount, collection);
        }

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">
        /// The entity id.
        /// </param>
        /// <returns>
        /// The entity specified by key <paramref name="id"/> or <c>null</c> if not found.
        /// </returns>
        public virtual TEntity GetById(TId id)
        {
            return this.Session.Get<TEntity>(id);
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        public virtual TEntity Save(TEntity domainEntity)
        {
            using (var transaction = new TransactionRequired(this.Session))
            {
                this.Session.Save(domainEntity);
                transaction.Commit();
            }

            return domainEntity;
        }

        /// <summary>
        /// Save or update the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        public virtual TEntity SaveOrUpdate(TEntity domainEntity)
        {
            using (var transaction = new TransactionRequired(this.Session))
            {
                this.Session.SaveOrUpdate(domainEntity);
                transaction.Commit();
            }

            return domainEntity;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="domainEntity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The persistent version of the entity.
        /// </returns>
        public virtual TEntity Update(TEntity domainEntity)
        {
            using (var transaction = new TransactionRequired(this.Session))
            {
                this.Session.Update(domainEntity);
                transaction.Commit();
            }

            return domainEntity;
        }
    }
}