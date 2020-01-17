using System.Collections.Generic;
using System.Threading.Tasks;
using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Services {
	public interface ISuitAlterationsService {
		Task<SuitAlteration> CreateSuitAlteration(SuitAlteration newAlteration);
		Task<SuitAlteration> SetSuitAlterationAsPaid(int alterationId);
		Task<IReadOnlyList<SuitAlteration>> GetSuitAlterationsBy(int customerId);
		Task<Customer> CreateCustomer(Customer customer);
		Task<IReadOnlyList<Customer>> GetAllCustomers();
	}
}