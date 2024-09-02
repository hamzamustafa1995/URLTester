using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLTester.Resources;

namespace URLTester.Application.Features.Commands.UpdateURL
{
	public class UpdateURLCommandValidator : AbstractValidator<UpdateURLCommand>
	{

        public UpdateURLCommandValidator()
        {
            RuleFor(x => x.Url).Cascade(CascadeMode.Stop)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage(Messages.URLShouldNotBeNullOrEmpty)
                .Must(x => x.StartsWith("http")).WithMessage(Messages.InvalidURL);
        }
	}
}
