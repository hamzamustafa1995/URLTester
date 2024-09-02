using MediatR;
using URLTester.Application.ViewModels;

namespace URLTester.Application.Features.Commands.CreateURL;

// the request is OriginalRequest
// the response is ResultViewModel 
public record CreateURLCommand : IRequest<ResultViewModel<URLViewModel>>
{
    public string? OriginalRequest { get; set; }
}