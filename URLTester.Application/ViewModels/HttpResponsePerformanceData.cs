using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLTester.Application.ViewModels
{
	public class HttpResponsePerformanceData
	{
		public string Content { get; set; }
		public long TotalElapsedMilliseconds { get; set; }
		public long DnsResolutionTime { get; set; }
		public long RequestSendTime { get; set; }
		public long ServerProcessingTime { get; set; }
		// Add other metrics if needed
	}
}
