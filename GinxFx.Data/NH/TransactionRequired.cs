// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionRequired.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the TransactionRequired type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GinxFx.Data.NH
{
    using System;

    using NHibernate;

    /// <summary>
    /// Indicates that a block of code requires a transaction.
    /// If a transaction are already open, this class will reuse it and the context which opened the transaction will
    /// be responsible for commit it or rollback it.
    /// Multiple <see cref="TransactionRequired"/> can be opened in a call context, the first (also the top level)
    /// transaction required will be responsible for commits the transaction.
    /// </summary>
    public sealed class TransactionRequired : IDisposable
    {
        /// <summary>
        /// A flag indicating that this instance is responsible for commit the transaction.
        /// </summary>
        private readonly bool shouldCommit;

        /// <summary>
        /// A flag indicating that this transaction was completed.
        /// </summary>
        private bool completed;

        /// <summary>
        /// The current transaction.
        /// </summary>
        private ITransaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRequired"/> class.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        public TransactionRequired(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.transaction = session.Transaction;

            if (IsOpenTransaction(this.transaction))
            {
                return;
            }

            this.transaction = session.BeginTransaction();
            this.shouldCommit = true;
        }

        /// <summary>
        /// Commits the transaction if this instance is responsible for it.
        /// </summary>
        public void Commit()
        {
            if (!this.shouldCommit)
            {
                return;
            }

            if (this.completed)
            {
                throw new InvalidOperationException(
                    "The current transaction is already committed. You should dispose the transaction.");
            }

            this.completed = true;

            try
            {
                if (IsOpenTransaction(this.transaction))
                {
                    this.transaction.Commit();
                    this.transaction = null;
                }
            }
            catch (StaleObjectStateException)
            {
                // TODO: Object was altered by other session
                this.Rollback();
                throw;
            }
            catch (HibernateException)
            {
                this.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.shouldCommit)
            {
                return;
            }

            this.Rollback();
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void Rollback()
        {
            if (!this.shouldCommit)
            {
                return;
            }

            this.completed = true;

            if (IsOpenTransaction(this.transaction))
            {
                this.transaction.Rollback();
            }

            this.transaction = null;
        }

        /// <summary>
        /// Check if a transaction is already open.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="transaction"/> is open, <c>false</c> otherwise.
        /// </returns>
        private static bool IsOpenTransaction(ITransaction transaction)
        {
            return transaction != null && transaction.IsActive && !transaction.WasCommitted
                   && !transaction.WasRolledBack;
        }
    }
}