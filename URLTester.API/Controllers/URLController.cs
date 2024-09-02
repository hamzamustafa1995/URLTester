using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using URLTester.Application.Features.Commands.CreateURL;
using URLTester.Application.Features.Commands.UpdateURL;
using URLTester.Application.Features.Queries.GetAllURLs;
using URLTester.Application.Features.Queries.GetURLByShortened;
using URLTester.Application.ViewModels;

namespace URLTester.API.Controllers;


[SwaggerTag("URLTester service")]
[Route("[controller]/[action]")]
public class URLController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation("Make a URL shorter")]
    [SwaggerResponse(StatusCodes.Status200OK, "Created", typeof(URLViewModel))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid URL", typeof(void))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "URL already exists", typeof(void))]
    [HttpPost]
    public async Task<IActionResult> Create(CreateURLCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
	[SwaggerOperation("Update Orginal URL")]
	[SwaggerResponse(StatusCodes.Status200OK, "Updated", typeof(URLViewModel))]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid URL", typeof(void))]
	[SwaggerResponse(StatusCodes.Status404NotFound, "URL Not Found", typeof(void))]
	[HttpPost]
	public async Task<IActionResult> Update(UpdateURLCommand request, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);
		return Ok(result);
	}
	[SwaggerOperation("Get all URLs")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retrieved", typeof(IReadOnlyList<URLViewModel>))]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllURLsQuery(), cancellationToken);
        return Ok(result);
    }

    [SwaggerOperation("Redirect short URL to original URL")]
    [SwaggerResponse(StatusCodes.Status301MovedPermanently, "Redirect", typeof(void))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "URL not found", typeof(void))]
	[HttpGet("/{input}")]
	public async Task<IActionResult> Get(string input, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetURLByShortenedQuery(input), cancellationToken);
        return RedirectPermanent(result);
    }
    [SwaggerOperation("Test Request Http Client")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid URL", typeof(void))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "URL not found", typeof(void))]
	[HttpPost]

	public async Task<IActionResult> SendRequest([FromBody] Application.ViewModels.HttpRequest input)
    {
        var result = await mediator.Send(new GetResponseHttpServiceQuery(input));
        return Ok(result);
    }
}
