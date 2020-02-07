using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SuitAlterations.Application.Customers.GetCustomerSuitAlterations;
using SuitAlterations.Application.SuitAlterations;
using SuitAlterations.Application.SuitAlterations.OrderPaid;
using SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.Customers.Events;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Database;
using SuitAlterations.Infrastructure.Domain;
using SuitAlterations.Infrastructure.Domain.Customers;
using SuitAlterations.Infrastructure.Domain.SuitAlterations;

namespace SuitAlterations.Application.UnitTests
{
	[TestFixture]
	public class ApplicationCommandQueryTests
	{
		private IUnitOfWork _unitOfWork;
		private ApplicationDbContext _dbContext;

		private Mock<IDomainEventsDispatcher> _domainEventsDispatcherMock;
		private Mock<IMapper> _mapperMock;

		private Customer CreateNewCustomer()
		{
			return Customer.CreateNew("John", "Wick");
		}

		[SetUp]
		public void SetUp()
		{
			_mapperMock = new Mock<IMapper>();
			_domainEventsDispatcherMock = new Mock<IDomainEventsDispatcher>();
			_dbContext = DbContextBuilder.BuildDbContext();
			// we will trigger UoW CommitAsync in tests every time as our command decorator dispatcher does under the hood
			_unitOfWork = new UnitOfWork(_dbContext, _domainEventsDispatcherMock.Object);
		}

		[TearDown]
		public void Dispose()
		{
			_dbContext.Dispose();
		}

		[Test]
		public async Task SuccessfullyPlacesCustomerOrderForNewSuitAlteration()
		{
			//Arrange
			var customerRepository = new CustomerRepository(_dbContext);
			var customer = CreateNewCustomer();
			await customerRepository.AddAsync(customer);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			var suitAlterationRepository = new SuitAlterationRepository(_dbContext);
			var customerOrders = await LoadCustomerOrders();
			//Pre-assert
			customerOrders.Should().BeEmpty();

			//Act
			var placeCustomerOrderCommandHandler = new PlaceCustomerOrderCommandHandler(customerRepository);
			await placeCustomerOrderCommandHandler.Handle(new PlaceCustomerOrderCommand(5,
				5,
				5,
				5,
				customer.Id.Value), CancellationToken.None);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			//Assert
			customerOrders = await LoadCustomerOrders();
			customerOrders.Should().ContainSingle();
			var createdOrder = customerOrders.Single();
			createdOrder.DomainEvents.Should().ContainSingle().And.BeEquivalentTo(new OrderPlacedDomainEvent(createdOrder.Id));
			createdOrder.Status.Should().Be(SuitAlterationStatus.Created);
			customer.SuitAlterations.Should().Contain(createdOrder);

			async Task<IReadOnlyList<SuitAlteration>> LoadCustomerOrders()
			{
				return await suitAlterationRepository.GetByCustomerIdAsync(customer.Id);
			}
		}

		[Test]
		public async Task UpdatesCustomerOrderStatusToPaidAfterTheReceivingOfOrderPaidNotification()
		{
			//Arrange
			var customerRepository = new CustomerRepository(_dbContext);
			var suitAlterationRepository = new SuitAlterationRepository(_dbContext);
			var customer = CreateNewCustomer();
			await customerRepository.AddAsync(customer);
			customer.PlaceOrder(5, 5, 5, 5);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			//Pre-assert
			var order = customer.SuitAlterations.Single();
			order.Status.Should().Be(SuitAlterationStatus.Created);

			//Act
			var markOrderAsPaidCommandHandler = new MarkOrderAsPaidCommandHandler(suitAlterationRepository);
			await markOrderAsPaidCommandHandler.Handle(new MarkOrderAsPaidCommand(order.Id), CancellationToken.None);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			//Assert
			SuitAlteration paidOrder = await suitAlterationRepository.GetByIdAsync(order.Id);
			order.DomainEvents.Should().ContainEquivalentOf(new OrderPaidDomainEvent(order.Id));
			paidOrder.Status.Should().Be(SuitAlterationStatus.Paid);
			order.DomainEvents.Count.Should().Be(2);
		}

		[Test]
		public async Task ThrowsExceptionWhenOrderStatusIsPaidAndWeTriesToSetTheSameStatusAgain()
		{
			//Arrange
			var customerRepository = new CustomerRepository(_dbContext);
			var suitAlterationRepository = new SuitAlterationRepository(_dbContext);

			var customer = CreateNewCustomer();
			await customerRepository.AddAsync(customer);

			customer.PlaceOrder(5, 5, 5, 5);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			//Act
			var order = customer.SuitAlterations.Single();
			var markOrderAsPaidCommandHandler = new MarkOrderAsPaidCommandHandler(suitAlterationRepository);
			await markOrderAsPaidCommandHandler.Handle(new MarkOrderAsPaidCommand(order.Id), CancellationToken.None);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			//Pre-assert
			order.DomainEvents.Should().ContainEquivalentOf(new OrderPaidDomainEvent(order.Id));
			//imagine that we got OrderPaidNotification second time
			Func<Task> action = async () => await markOrderAsPaidCommandHandler.Handle(new MarkOrderAsPaidCommand(order.Id), CancellationToken.None);

			//Assert
			action.Should().Throw<BusinessRuleValidationException>().WithMessage(
				$"Order has inconsistent status to be marked as paid. Status - {order.Status.ToString()}");
			order.DomainEvents.Count.Should().Be(2);
		}


		[Test]
		public void ThrowsExceptionWhenEntityIdNotFound()
		{
			//Arrange
			var suitAlterationRepository = new SuitAlterationRepository(_dbContext);
			var orderId = new SuitAlterationId(Guid.NewGuid());

			//Act
			Func<Task> action = async () => await suitAlterationRepository.GetByIdAsync(orderId);

			//Assert
			action.Should().Throw<EntityNotFoundException>()
				.WithMessage($"Entity with id={orderId} not found");
		}


		[Test]
		public async Task GetsExpectedSuitAlterationByCustomerId()
		{
			//Arrange
			var suitAlterationRepository = new SuitAlterationRepository(_dbContext);
			var customerRepository = new CustomerRepository(_dbContext);

			var expectedCustomer = await RegisterCustomer();
			expectedCustomer.PlaceOrder(5, 5, 5, 5);

			var anotherCustomer = await RegisterCustomer();
			anotherCustomer.PlaceOrder(1, 1, 1, 1);
			await _unitOfWork.CommitAsync(CancellationToken.None);

			var expectedOrder = expectedCustomer.SuitAlterations.Single();
			SuitAlterationDto expectedCustomerSuitAlteration = null;
			_mapperMock.Setup(x => x.Map<SuitAlterationDto>(expectedOrder))
				.Returns((SuitAlteration suitAlteration) =>
				{
					expectedCustomerSuitAlteration = new SuitAlterationDto()
					{
						Id = suitAlteration.Id.Value
					};
					return expectedCustomerSuitAlteration;
				});

			//Act
			var customerOrdersQueryHandler = new GetCustomerSuitAlterationsQueryHandler(suitAlterationRepository, _mapperMock.Object);
			IReadOnlyList<SuitAlterationDto> customerSuitAlterations =
				await customerOrdersQueryHandler.Handle(new GetCustomerSuitAlterationsQuery(expectedCustomer.Id.Value), CancellationToken.None);

			//Assert
			customerSuitAlterations.Should().ContainSingle().Which.Id.Should().Be(expectedCustomerSuitAlteration.Id);

			async Task<Customer> RegisterCustomer()
			{
				var customer = CreateNewCustomer();
				await customerRepository.AddAsync(customer);
				return customer;
			}
		}
	}
}