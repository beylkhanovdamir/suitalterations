using System;

namespace SuitAlterations.Domain.SeedWork
{
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException()
		{
		}

		public EntityNotFoundException(string message) : base(message)
		{
		}
	}
}