namespace SuitAlterations.ServiceBusTopic {
	public class AzureServiceBusConfiguration {
		public string ConnectionString { get; set; }
		public AzureServiceBusTopic Topic { get; set; }
	}

	public class AzureServiceBusTopic {
		public string Path { get; set; }
		public string SubscriptionName { get; set; }
	}
}