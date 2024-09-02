using MediatR;
using StackExchange.Profiling;
using System.Diagnostics;
using URLTester.Application.Exceptions;
using URLTester.Application.Features.Queries.GetAllURLs;
using URLTester.Application.ViewModels;
using URLTester.Domain.Repositories;
using URLTester.Resources;

namespace URLTester.Application.Features.Queries.GetURLByShortened;

public record GetResponseHttpServiceHandler(IHttpService _repository) : IRequestHandler<GetResponseHttpServiceQuery, ResultViewModel<string>>
{
    public async Task<ResultViewModel<string>> Handle(GetResponseHttpServiceQuery request, CancellationToken cancellationToken)
	{
		if (request.HttpRequestViewModel == null)
		{
			// Handle the case where HttpRequestViewModel is null
			throw new ArgumentNullException(nameof(request.HttpRequestViewModel));
		}
		var content = string.Empty;
		var stopwatch = Stopwatch.StartNew();
		using (MiniProfiler.Current.Step("SendRequestAsync - HTTP Request"))
		{
			content = await _repository.SendRequestAsync(request.HttpRequestViewModel);
		}
		stopwatch.Stop();
		var elapsedMilliseconds = stopwatch.ElapsedMilliseconds / 1000.0; 

		// Example of processing the response (if needed)
		return ResultViewModel<string>.OK(content, Messages.Success, elapsedMilliseconds);
	}
}
