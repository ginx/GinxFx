// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entity.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the Entity type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Types
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The abstract entity object class, which contains the core methods used to compare entity object in memory.
    /// </summary>
    /// <typeparam name="TId">
    /// Defines the type of database primary key.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// Defines the entity object which will extends this class.
    /// </typeparam>
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class Entity<TId, TEntity>
        where TEntity : Entity<TId, TEntity>
    {
        /// <summary>
        /// The transient hash code to be used in case the entity are not persisted.
        /// </summary>
        private int? transientHashCode;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        [DataMember]
        public virtual TId Id { get; set; }

        /// <summary>
        /// Implements the operator == (Equal).
        /// </summary>
        /// <param name="first">
        /// The first entity.
        /// </param>
        /// <param name="second">
        /// The second entity.
        /// </param>
        /// <returns>
        /// <c>true</c> if entities are equal, <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==(Entity<TId, TEntity> first, Entity<TId, TEntity> second)
        {
            return Entity<TId, TEntity>.Equals(first, second);
        }

        /// <summary>
        /// Implements the operator != (Not Equals).
        /// </summary>
        /// <param name="first">
        /// The first entity.
        /// </param>
        /// <param name="second">
        /// The second entity.
        /// </param>
        /// <returns>
        /// <c>true</c> if entities are not equal, <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=(Entity<TId, TEntity> first, Entity<TId, TEntity> second)
        {
            return !Entity<TId, TEntity>.Equals(first, second);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current
        /// <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to be compared.
        /// </param>
        /// <returns>
        /// <c>true</c> if objects are equal, <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            var x = obj as TEntity;

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (this.IsTransient() && x.IsTransient())
            {
                return object.ReferenceEquals(this, x);
            }

            return this.Id.Equals(x.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.GetHashCodeInternal();
        }

        /// <summary>
        /// Determines whether this entity is transient.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this entity is transient; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsTransient()
        {
            return object.Equals(this.Id, default(TId));
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <c>string</c> representation of the entity.
        /// </returns>
        public override string ToString()
        {
            return this.GetType().Name + " = " + (this.IsTransient() ? "(Transient)" : this.Id.ToString());
        }

        /// <summary>
        /// Determines whether the first <see cref="Entity{TId,TObject}"/> is equal to the second
        /// <see cref="Entity{TId,TObject}"/>.
        /// </summary>
        /// <param name="first">
        /// The first object to be compared.
        /// </param>
        /// <param name="second">
        /// The second object to be compared.
        /// </param>
        /// <returns>
        /// <c>true</c> if objects are equal, <c>false</c> otherwise.
        /// </returns>
        private static bool Equals(Entity<TId, TEntity> first, Entity<TId, TEntity> second)
        {
            if (object.ReferenceEquals(first, null) && object.ReferenceEquals(second, null))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null) || object.ReferenceEquals(second, null))
            {
                return false;
            }

            if (first.IsTransient() && second.IsTransient())
            {
                return object.ReferenceEquals(first, second);
            }

            return first.Id.Equals(second.Id);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// If persistent, uses Id; if transient, uses temporary code.
        /// (This will not work if the entity is evicted and reloaded).
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        private int GetHashCodeInternal()
        {
            if (this.transientHashCode.HasValue)
            {
                return this.transientHashCode.Value;
            }

            if (this.IsTransient())
            {
                this.transientHashCode = base.GetHashCode();
                return this.transientHashCode.Value;
            }

            return this.Id.GetHashCode();
        }
    }
}