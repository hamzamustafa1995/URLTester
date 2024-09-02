using MediatR;
using URLTester.Application.ViewModels;

namespace URLTester.Application.Features.Queries.GetAllURLs;

public record GetAllURLsQuery : IRequest<ResultViewModel<IReadOnlyList<URLViewModel>>>;
