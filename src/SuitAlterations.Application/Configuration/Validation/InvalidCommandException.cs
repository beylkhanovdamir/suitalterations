using System;

namespace SuitAlterations.Application.Configuration.Validation
{
	public class InvalidCommandException: Exception
	{
		public InvalidCommandException(string message) : base(message)
		{
		}
	}
}