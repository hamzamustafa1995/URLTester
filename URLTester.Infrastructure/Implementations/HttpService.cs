using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using URLTester.Application.ViewModels;
using URLTester.Domain.Models;
using URLTester.Domain.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace URLTester.Infrastructure.Implementations
{
	public class HttpService : IHttpService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public HttpService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}


		public async Task<string> SendRequestAsync(HttpRequest httpRequest)
		{
			var client = _httpClientFactory.CreateClient("ProfiledHttpClient");
            HttpMethod httpMethod = string.Equals(httpRequest.Method, "Get", StringComparison.OrdinalIgnoreCase)
                ? HttpMethod.Get
                : HttpMethod.Post; 
			var request = new HttpRequestMessage(httpMethod, httpRequest.Url);

			// Set the Authorization header if a token is provided
			if (!string.IsNullOrEmpty(httpRequest.Token))
			{
				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", httpRequest.Token);
			}

			if (httpRequest.Data != null)
			{
				request.Content = new StringContent(JsonConvert.SerializeObject(httpRequest.Data), Encoding.UTF8, "application/json");
			}
			var response = await client.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();
			response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not successful

			return content;

		}
	}

}
