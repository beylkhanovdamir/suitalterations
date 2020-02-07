using System;
using System.Threading.Tasks;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations
{
	public class NotifierService : INotifierService
	{
		public event Func<SuitAlterationId, Task> OrderPaidNotification;
		public event Func<SuitAlterationId, Task> OrderPlacedNotification;
		
		public async Task OnOrderPaidNotification(SuitAlterationId orderId)
		{
			if (OrderPaidNotification != null)
			{
				await OrderPaidNotification.Invoke(orderId);
			}
		}
		public async Task OnOrderPlacedNotification(SuitAlterationId orderId)
		{
			if (OrderPlacedNotification != null)
			{
				await OrderPlacedNotification.Invoke(orderId);
			}
		}
	}
}