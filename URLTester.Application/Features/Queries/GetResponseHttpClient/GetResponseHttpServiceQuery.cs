using MediatR;
using System.Web;
using URLTester.Application.ViewModels;

namespace URLTester.Application.Features.Queries.GetURLByShortened;

public record GetResponseHttpServiceQuery(HttpRequest HttpRequestViewModel) : IRequest<ResultViewModel<string>>;
