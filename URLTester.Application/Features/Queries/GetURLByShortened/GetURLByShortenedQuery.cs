using MediatR;
using System.Web;

namespace URLTester.Application.Features.Queries.GetURLByShortened;

public record GetURLByShortenedQuery : IRequest<string>
{
    public string Shortened { get; set; }
    public GetURLByShortenedQuery(string shortened)
	{  // Decode the URL-encoded parameter
		
		this.Shortened = decodeShortened( shortened);
    }
	public string decodeShortened(string shortened)
	{
		// Decode the URL-encoded input parameter
		var decodedInput = HttpUtility.UrlDecode(shortened);

		// Determine if the input is a full URL or just a short URL
		string shortUrl;

		if (Uri.TryCreate(decodedInput, UriKind.Absolute, out var uri) &&
			(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
		{
			// If it's a full URL, extract the last segment (short URL)
			shortUrl = uri.AbsolutePath.TrimStart('/'); // Remove leading slash if necessary
		}
		else
		{
			// If it's not a full URL, assume it's the short URL
			shortUrl = decodedInput;
		}
		return shortUrl;
	}
}