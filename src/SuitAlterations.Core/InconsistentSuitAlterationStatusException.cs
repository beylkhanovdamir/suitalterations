using System;

namespace SuitAlterations.Core {
	public class InconsistentSuitAlterationStatusException : Exception {
		public InconsistentSuitAlterationStatusException() { }
		public InconsistentSuitAlterationStatusException(string message) : base(message) { }
	}
}