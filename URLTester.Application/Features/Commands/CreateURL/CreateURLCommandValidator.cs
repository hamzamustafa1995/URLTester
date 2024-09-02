using FluentValidation;
using URLTester.Resources;

namespace URLTester.Application.Features.Commands.CreateURL;

public class CreateURLCommandValidator : AbstractValidator<CreateURLCommand>
{
    public CreateURLCommandValidator()
    {
		//OriginalRequest is the paramter that request in CreateURLCommand
		RuleFor(x => x.OriginalRequest).Cascade(CascadeMode.Stop)
            .Must(x => !string.IsNullOrEmpty(x)).WithMessage(Messages.URLShouldNotBeNullOrEmpty)
            .Must(x => x.StartsWith("http")).WithMessage(Messages.InvalidURL);
    }
}
