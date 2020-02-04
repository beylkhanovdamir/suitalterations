using System;

namespace SuitAlterations.Domain.SeedWork
{
	public abstract class BaseValueIdType : IEquatable<BaseValueIdType>
	{
		public Guid Value { get; }

		protected BaseValueIdType(Guid value)
		{
			Value = value;
		}

		public bool Equals(BaseValueIdType other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BaseValueIdType) obj);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator ==(BaseValueIdType left, BaseValueIdType right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(BaseValueIdType left, BaseValueIdType right)
		{
			return !Equals(left, right);
		}
	}
}