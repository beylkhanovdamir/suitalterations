using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace SuitAlterations.Application.Configuration.Validation
{
	public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var errors = _validators
				.Select(v => v.Validate(request))
				.SelectMany(result => result.Errors)
				.Where(error => error != null)
				.ToList();

			if (!errors.Any()) return next();

			var errorBuilder = new StringBuilder();
			errorBuilder.AppendLine("Command doesn't pass validation. Details: ");

			foreach (var error in errors)
			{
				errorBuilder.AppendLine(error.ErrorMessage);
			}

			throw new InvalidCommandException(errorBuilder.ToString());
		}
	}
}