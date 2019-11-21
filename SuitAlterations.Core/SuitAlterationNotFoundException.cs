using System;

namespace SuitAlterations.Core {
	public class SuitAlterationNotFoundException : Exception {
		public SuitAlterationNotFoundException() { }
		public SuitAlterationNotFoundException(string message) : base(message) { }
	}
}