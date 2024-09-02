using MediatR;
using URLTester.Application.Exceptions;
using URLTester.Domain.Repositories;
using URLTester.Resources;

namespace URLTester.Application.Features.Queries.GetURLByShortened;

public class GetURLByShortenedQueryHandler(IURLRepository repository) : IRequestHandler<GetURLByShortenedQuery, string>
{
    public async Task<string> Handle(GetURLByShortenedQuery request, CancellationToken cancellationToken)
    {
        var url = await repository.GetByShortenedAsync(request.Shortened, cancellationToken)
            ?? throw new NotFoundException(Messages.NotFoundShortenedURL);

        url.Visit();
        await repository.UpdateAsync(url, cancellationToken);

        return url.Original;
    }
}
