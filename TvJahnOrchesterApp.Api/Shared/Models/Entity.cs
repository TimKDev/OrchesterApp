using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Domain.Common.Models
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : ValueObject
    {
        public TId Id { get; protected set; }
        protected Entity(TId id)
        {
            Id = id;
        }

#pragma warning disable CS8618 
        protected Entity() { }
#pragma warning restore CS8618 

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> entity && Id.Equals(entity.Id);
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Entity<TId>? other)
        {
            return Equals((object?)other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
