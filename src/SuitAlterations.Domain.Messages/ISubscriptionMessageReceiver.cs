using System.Threading.Tasks;

namespace SuitAlterations.Domain.Messages
{
	public interface ISubscriptionMessageReceiver<in TMessageFilter> where TMessageFilter : ITopicMessageFilter
	{
		Task RegisterMessageReceivingAsync(TMessageFilter message);

		Task StopReceivingMessagesAsync();
	}
}