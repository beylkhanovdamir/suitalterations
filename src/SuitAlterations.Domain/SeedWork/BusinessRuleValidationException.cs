using System;

namespace SuitAlterations.Domain.SeedWork
{
	public class BusinessRuleValidationException : Exception
	{
		public BusinessRuleValidationException()
		{
		}

		public BusinessRuleValidationException(string message) : base(message)
		{
		}
	}
}