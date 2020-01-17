using System;

namespace SuitAlterations.Core {
	public class CustomerNotFoundException : Exception {
		public CustomerNotFoundException() { }
		public CustomerNotFoundException(string message) : base(message) { }
	}
}