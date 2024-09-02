using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLTester.Application.Configs;
using URLTester.Application.Exceptions;
using URLTester.Application.ViewModels;
using URLTester.Domain.Helpers;
using URLTester.Domain.Models;
using URLTester.Domain.Repositories;
using URLTester.Resources;

namespace URLTester.Application.Features.Commands.UpdateURL
{
	public class UpdateURLCommandHandler : IRequestHandler<UpdateURLCommand, ResultViewModel<bool>>
	{
		IURLRepository _repository;
		IOptions<Settings> _optionSettings;
		private readonly Settings settings;
		public UpdateURLCommandHandler(IURLRepository repository, IOptions<Settings> optionSettings)
		{
			this._repository = repository;
			this._optionSettings = optionSettings;
			this.settings = _optionSettings.Value;
		}
        public async Task<ResultViewModel<bool>> Handle(UpdateURLCommand request, CancellationToken cancellationToken)
		{
			var stopwatch = Stopwatch.StartNew();

	
			var tmp = await _repository.GetByURLIdAsync(request.UrlId, cancellationToken);

			if (tmp is null)
			{
				throw new NotFoundException(Messages.InvalidURL);
			}
			tmp.Original = request.Url;
			tmp.Shortened = MethodHelper.GenerateShortened(true);

			await _repository.UpdateAsync(tmp, cancellationToken);
			stopwatch.Stop();
			var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			return ResultViewModel<bool>.OK(true, Messages.Success, elapsedMilliseconds);
		}
	}
}
