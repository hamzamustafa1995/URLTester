using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using URLTester.Domain.Models;

namespace URLTester.Application.ViewModels
{
	public class HttpRequest
	{
		public string Url { get; set; }
		public string Method { get; set; }
		public object Data { get; set; }
		public string Token { get; set; }

		public HttpRequest()
		{
		}

		public HttpRequest(string url, string method = null, object data = null, string token = null)
		{
			Url = Uri.UnescapeDataString(url);
			Method = method;
			Data = data;
			Token = token;
		}
	}

}
