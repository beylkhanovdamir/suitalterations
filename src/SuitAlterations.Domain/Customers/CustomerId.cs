using System;
using SuitAlterations.Domain.SharedKernel;

namespace SuitAlterations.Domain.Customers
{
	public class CustomerId : BaseValueIdType
	{
		public CustomerId(Guid value) : base(value)
		{
		}
	}
}