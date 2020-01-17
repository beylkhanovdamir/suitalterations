using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SuitAlterations.Core.Common;
using SuitAlterations.Core.Data;
using SuitAlterations.Core.Entities;
using SuitAlterations.Core.Services;

namespace SuitAlterations.Core.UnitTests {
	[TestFixture]
	public class SuitAlterationsServiceTests {
		private ISuitAlterationsService _suitAlterationsService;

		private Customer SomeRegularCustomer => new Customer { FirstName = "John", LastName = "Wick" };

		private SuitAlteration SomeSuitAlteration(int customerId = 1, SuitAlterationStatus suitAlterationStatus = SuitAlterationStatus.Created) {
			return new SuitAlteration {
				CustomerId = customerId,
				LeftSleeveLength = 5,
				LeftTrouserLength = 5,
				RightSleeveLength = 5,
				RightTrouserLength = 5,
				Status = suitAlterationStatus
			};
		}

		/// <summary>
		///     Entity Framework Core provides in-memory data provider and we no need to mock our DbContext or repositories.
		///     Moreover, we can reach more testability of our logic by this way
		/// </summary>
		private ApplicationDbContext BuildDbContext() {
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			                                                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			                                                 .Options;

			return new ApplicationDbContext(options);
		}

		[Test]
		public async Task SuccessfullyCreatesNewSuitAlterationInDbAndReturnsCreatedEntity() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var customerRepository = new CustomerRepository(context);
				Customer customer = await customerRepository.Create(SomeRegularCustomer);
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), customerRepository);

				SuitAlteration createdSuitAlteration = await _suitAlterationsService.CreateSuitAlteration(SomeSuitAlteration(customer.Id));

				createdSuitAlteration.Id.Should().BePositive();
				createdSuitAlteration.Status.Should().Be(SuitAlterationStatus.Created);
				createdSuitAlteration.Customer.Should().BeEquivalentTo(customer);
			}
		}

		[Test]
		public async Task UpdatesSuitAlterationStatusToPaidInDbAfterPaidSuitAlterationNotificationWasReceived() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var suitAlterationRepository = new SuitAlterationRepository(context);
				SuitAlteration createdSuitAlteration = await suitAlterationRepository.Create(SomeSuitAlteration());
				_suitAlterationsService = new SuitAlterationsService(suitAlterationRepository, new CustomerRepository(context));

				SuitAlteration paidSuitAlteration = await _suitAlterationsService.SetSuitAlterationAsPaid(createdSuitAlteration.Id);

				paidSuitAlteration.Status.Should().Be(SuitAlterationStatus.Paid);
				paidSuitAlteration.Should().BeEquivalentTo(createdSuitAlteration);
			}
		}

		[TestCase(SuitAlterationStatus.Done)]
		[TestCase(SuitAlterationStatus.Paid)]
		public async Task ThrowsExceptionWhenCustomerSuitAlterationStatusIsInconsistent(SuitAlterationStatus inconsistentSuitAlterationStatus) {
			using (ApplicationDbContext context = BuildDbContext()) {
				var suitAlterationRepository = new SuitAlterationRepository(context);
				SuitAlteration createdSuitAlteration = await suitAlterationRepository.Create(SomeSuitAlteration(suitAlterationStatus: inconsistentSuitAlterationStatus));
				_suitAlterationsService = new SuitAlterationsService(suitAlterationRepository, new CustomerRepository(context));

				Func<Task> action = async () => await _suitAlterationsService.SetSuitAlterationAsPaid(createdSuitAlteration.Id);

				action.Should().Throw<InconsistentSuitAlterationStatusException>().WithMessage($"Suit alteration record has inconsistent status - {inconsistentSuitAlterationStatus.ToString()}");
			}
		}

		[Test]
		public void ThrowsExceptionWhenCustomerIdNotFoundDuringCreatingSuitAlteration() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));
				var nonExistingCustomerId = 100;
				Func<Task> action = async () => await _suitAlterationsService.CreateSuitAlteration(SomeSuitAlteration(customerId: nonExistingCustomerId));

				action.Should().Throw<CustomerNotFoundException>().WithMessage($"Customer with id={nonExistingCustomerId} not found");
			}
		}

		[Test]
		public void ThrowsExceptionWhenCustomerSuitAlterationNotFound() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));
				int nonExistingAlterationId = 100;

				Func<Task> action = async () => await _suitAlterationsService.SetSuitAlterationAsPaid(nonExistingAlterationId);

				action.Should().Throw<SuitAlterationNotFoundException>().WithMessage($"Suit alteration with id={nonExistingAlterationId} not found");
			}
		}

		[Test]
		public async Task GetsExpectedSuitAlterationByCustomerId() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var suitAlterationRepository = new SuitAlterationRepository(context);
				var customerRepository = new CustomerRepository(context);

				await CreateSuitAlterationForAnotherCustomer(suitAlterationRepository, customerRepository);

				SuitAlteration expectedCustomerSuitAlteration = await CreateSuitAlterationForExpectedCustomer(suitAlterationRepository, customerRepository);

				_suitAlterationsService = new SuitAlterationsService(suitAlterationRepository, customerRepository);

				IReadOnlyList<SuitAlteration> customerSuitAlterations = await _suitAlterationsService.GetSuitAlterationsBy(expectedCustomerSuitAlteration.CustomerId);
				customerSuitAlterations.Should().ContainSingle().Which.Should().BeEquivalentTo(expectedCustomerSuitAlteration);
			}

			async Task CreateSuitAlterationForAnotherCustomer(SuitAlterationRepository suitAlterationRepository, CustomerRepository customerRepository) {
				Customer anotherCustomer = await customerRepository.Create(SomeRegularCustomer);
				await suitAlterationRepository.Create(SomeSuitAlteration(anotherCustomer.Id));
			}

			async Task<SuitAlteration> CreateSuitAlterationForExpectedCustomer(SuitAlterationRepository suitAlterationRepository, CustomerRepository customerRepository) {
				Customer expectedCustomer = await customerRepository.Create(SomeRegularCustomer);
				return await suitAlterationRepository.Create(SomeSuitAlteration(expectedCustomer.Id));
			}
		}

		[Test]
		public async Task SuccessfullyCreatesNewCustomerInDbAndReturnsCreatedEntity() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));

				Customer createdCustomer = await _suitAlterationsService.CreateCustomer(SomeRegularCustomer);

				createdCustomer.Id.Should().BePositive();
			}
		}

		[Test]
		public async Task GetsSingleExistingCustomerInCustomersDbSet() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var customerRepository = new CustomerRepository(context);
				Customer existingCustomer = await customerRepository.Create(SomeRegularCustomer);

				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));

				IReadOnlyList<Customer> customers = await _suitAlterationsService.GetAllCustomers();

				customers.Should().ContainSingle().Which.Should().BeEquivalentTo(existingCustomer);
			}
		}
	}
}