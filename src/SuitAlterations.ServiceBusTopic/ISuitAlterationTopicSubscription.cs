using System.Threading.Tasks;

namespace SuitAlterations.ServiceBusTopic {
	public interface ISuitAlterationTopicSubscription {
		void RegisterMessageReceivingHandler();
		Task StopReceivingMessages();
	}
}