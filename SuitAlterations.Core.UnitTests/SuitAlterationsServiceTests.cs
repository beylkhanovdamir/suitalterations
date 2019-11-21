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
		private const int CustomerId = 1;

		private SuitAlteration SomeSuitAlteration(int customerId = CustomerId, SuitAlterationStatus suitAlterationStatus = SuitAlterationStatus.Created) {
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
		/// Entity Framework Core provides in-memory data provider and we no need to mock our DbContext or repositories. Moreover, we can reach more testability of our logic by this way
		/// </summary>
		private ApplicationDbContext BuildDbContext() {
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			                                                 .UseInMemoryDatabase(databaseName: "suitAlterationsDb")
			                                                 .Options;

			return new ApplicationDbContext(options);
		}

		[Test]
		public async Task SuccessfullyCreatesNewSuitAlterationInDbAndReturnsCreatedEntity() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));

				SuitAlteration createdSuitAlteration =
					await _suitAlterationsService.CreateSuitAlteration(SomeSuitAlteration());

				createdSuitAlteration.Id.Should().BePositive();
				createdSuitAlteration.Status.Should().Be(SuitAlterationStatus.Created);
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
			}
		}

		[TestCase(SuitAlterationStatus.Done)]
		[TestCase(SuitAlterationStatus.Paid)]
		public async Task ThrowsExceptionWhenCustomerSuitAlterationStatusIsInconsistent(SuitAlterationStatus suitAlterationStatus) {
			using (ApplicationDbContext context = BuildDbContext()) {
				var suitAlterationRepository = new SuitAlterationRepository(context);
				SuitAlteration createdSuitAlteration = await suitAlterationRepository.Create(SomeSuitAlteration(suitAlterationStatus: suitAlterationStatus));
				_suitAlterationsService = new SuitAlterationsService(suitAlterationRepository, new CustomerRepository(context));

				Func<Task> action = async () => await _suitAlterationsService.SetSuitAlterationAsPaid(createdSuitAlteration.Id);

				action.Should().Throw<InconsistentSuitAlterationStatusException>().WithMessage($"Suit alteration record has inconsistent status - {suitAlterationStatus.ToString()}");
			}
		}

		[Test]
		public void ThrowsExceptionWhenCustomerSuitAlterationNotFound() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));
				int alterationId = 100;

				Func<Task> action = async () => await _suitAlterationsService.SetSuitAlterationAsPaid(alterationId);

				action.Should().Throw<SuitAlterationNotFoundException>().WithMessage($"Suit alteration with id={alterationId} not found");
			}
		}

		[Test]
		public async Task GetsSuitAlterationsByCustomerId() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var suitAlterationRepository = new SuitAlterationRepository(context);
				SuitAlteration customerSuitAlteration = await suitAlterationRepository.Create(SomeSuitAlteration());
				await suitAlterationRepository.Create(SomeSuitAlteration(customerId: 100));
				_suitAlterationsService = new SuitAlterationsService(suitAlterationRepository, new CustomerRepository(context));

				IReadOnlyList<SuitAlteration> customerSuitAlterations = await _suitAlterationsService.GetSuitAlterationsBy(CustomerId);

				customerSuitAlterations.Should().ContainSingle().Which.Should().BeEquivalentTo(customerSuitAlteration);
			}
		}

		[Test]
		public async Task SuccessfullyCreatesNewCustomerInDbAndReturnsCreatedEntity() {
			using (ApplicationDbContext context = BuildDbContext()) {
				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));

				Customer createdCustomer = await _suitAlterationsService.CreateCustomer(new Customer { FirstName = "John", LastName = "Wick" });

				createdCustomer.Id.Should().BePositive();
			}
		}

		[Test]
		public async Task GetsSingleExistingCustomerInCustomersDbSet() {
			using (ApplicationDbContext context = BuildDbContext()) {
				var customerRepository = new CustomerRepository(context);
				Customer existingCustomer = await customerRepository.Create(new Customer { Id = 100 });

				_suitAlterationsService = new SuitAlterationsService(new SuitAlterationRepository(context), new CustomerRepository(context));

				IReadOnlyList<Customer> customers = await _suitAlterationsService.GetAllCustomers();

				customers.Should().ContainSingle().Which.Should().BeEquivalentTo(existingCustomer);
			}
		}
	}
}