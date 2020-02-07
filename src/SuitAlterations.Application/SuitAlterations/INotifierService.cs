using System;
using System.Threading.Tasks;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations
{
	public interface INotifierService
	{
		event Func<SuitAlterationId, Task> OrderPlacedNotification;
		Task OnOrderPlacedNotification(SuitAlterationId orderId);
		
		event Func<SuitAlterationId, Task> OrderPaidNotification;
		Task OnOrderPaidNotification(SuitAlterationId orderId);
	}
}