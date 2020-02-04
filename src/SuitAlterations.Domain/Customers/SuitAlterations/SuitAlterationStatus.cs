using System.ComponentModel;

namespace SuitAlterations.Domain.Customers.SuitAlterations
{
	public enum SuitAlterationStatus
	{
		[Description("Created")] 
		Created = 0,
		[Description("Paid")] 
		Paid = 1,
		[Description("Done")] 
		Done = 2
	}
}