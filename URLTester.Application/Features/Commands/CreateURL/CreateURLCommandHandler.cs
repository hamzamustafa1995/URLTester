using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using URLTester.Application.Configs;
using URLTester.Application.Exceptions;
using URLTester.Application.ViewModels;
using URLTester.Domain.Models;
using URLTester.Domain.Repositories;
using URLTester.Resources;

namespace URLTester.Application.Features.Commands.CreateURL;

public class CreateURLCommandHandler: IRequestHandler<CreateURLCommand, ResultViewModel<URLViewModel>>
{
    IURLRepository _repository;
    IOptions<Settings> _optionSettings ;
    private readonly Settings settings;

    public CreateURLCommandHandler(IURLRepository repository, IOptions<Settings> optionSettings)
    {
        this._repository = repository;
        this._optionSettings = optionSettings;
        this.settings = _optionSettings.Value;
	}
    public async Task<ResultViewModel<URLViewModel>> Handle(CreateURLCommand request, CancellationToken cancellationToken)
	{
		var stopwatch = Stopwatch.StartNew();

		var tmp = await _repository.GetByOriginalAsync(request.OriginalRequest, cancellationToken);
        if (tmp is not null)
        {
            throw new ConflictException(Messages.RepetitiveOriginalURL);
        }

        var url = URL.Create(request.OriginalRequest, true);
        await _repository.AddAsync(url, cancellationToken);
        URLViewModel URLViewDto = url.ToViewModel(settings.BaseURL);
		stopwatch.Stop();
		var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		return ResultViewModel<URLViewModel>.OK(URLViewDto, Messages.Success, elapsedMilliseconds);
    }
}
