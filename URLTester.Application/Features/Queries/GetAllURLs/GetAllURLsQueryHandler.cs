using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using URLTester.Application.Configs;
using URLTester.Application.ViewModels;
using URLTester.Domain.Repositories;
using URLTester.Resources;

namespace URLTester.Application.Features.Queries.GetAllURLs;

public class GetAllURLsQueryHandler(IURLRepository repository, IOptions<Settings> optionSettings) : IRequestHandler<GetAllURLsQuery, ResultViewModel<IReadOnlyList<URLViewModel>>>
{
    private readonly Settings settings = optionSettings.Value;

    public async Task<ResultViewModel<IReadOnlyList<URLViewModel>>> Handle(GetAllURLsQuery request, CancellationToken cancellationToken)
	{
		var stopwatch = Stopwatch.StartNew();
		var urls = await repository.GetAllAsync(cancellationToken);
        var viewModels = urls.ToViewModel(settings.BaseURL);
		stopwatch.Stop();
		var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

		return ResultViewModel<IReadOnlyList<URLViewModel>>.OK(viewModels, Messages.Success, elapsedMilliseconds);
    }
}
