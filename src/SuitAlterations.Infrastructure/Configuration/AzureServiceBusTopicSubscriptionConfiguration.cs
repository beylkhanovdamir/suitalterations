namespace SuitAlterations.Infrastructure.Configuration
{
	public sealed class AzureServiceBusTopicSubscriptionConfiguration
	{
		public string ConnectionString { get; set; }
		public string TopicPath { get; set; }
		public string SubscriptionName { get; set; }
	}
}