using MediatR;

namespace SuitAlterations.ServiceBusTopic.Notifications {
	public class PaidSuitAlteration : INotification {
		public int AlterationId { get; set; }
	}
}