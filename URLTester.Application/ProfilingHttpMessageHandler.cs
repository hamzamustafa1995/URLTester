using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLTester.Application
{
	public class ProfilingHttpMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			using (MiniProfiler.Current.Step("HTTP Request: " + request.RequestUri))
			{
				return await base.SendAsync(request, cancellationToken);
			}
		}
	}
}
