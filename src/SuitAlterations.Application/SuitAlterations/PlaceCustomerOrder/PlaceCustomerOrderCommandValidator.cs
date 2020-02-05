using FluentValidation;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class PlaceCustomerOrderCommandValidator : AbstractValidator<PlaceCustomerOrderCommand>
	{
		private const string AlterationValidationMessage = "Can be up to plus or minus 5 cm";

		public PlaceCustomerOrderCommandValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer Id cannot be empty");
			RuleFor(x => x.LeftSleeveLength)
				.InclusiveBetween(-5, 5)
				.WithMessage(AlterationValidationMessage);
			RuleFor(x => x.RightSleeveLength)
				.InclusiveBetween(-5, 5)
				.WithMessage(AlterationValidationMessage);
			RuleFor(x => x.LeftTrouserLength)
				.InclusiveBetween(-5, 5)
				.WithMessage(AlterationValidationMessage);
			RuleFor(x => x.RightTrouserLength)
				.InclusiveBetween(-5, 5)
				.WithMessage(AlterationValidationMessage);
		}
	}
}