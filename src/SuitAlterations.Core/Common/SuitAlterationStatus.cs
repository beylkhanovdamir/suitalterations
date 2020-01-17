using System.ComponentModel;

namespace SuitAlterations.Core.Common {
	public enum SuitAlterationStatus {
		[Description("Created")]
		Created = 0,
		[Description("Paid")]
		Paid = 1,
		[Description("Done")]
		Done = 2
	}
}