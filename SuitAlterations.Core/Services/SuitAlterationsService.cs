using System;
using System.Threading.Tasks;

namespace SuitAlterations.Core.Services {
	public class SuitAlterationsService : ISuitAlterationsService {
		public Task SetSuitAlterationAsPaid(int alterationId) {
			// todo: update alteration status in db
			Console.WriteLine(alterationId);
			return Task.CompletedTask;
		}
	}
}