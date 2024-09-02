using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLTester.Application.ViewModels;

namespace URLTester.Domain.Repositories
{
	public interface IHttpService
	{
		Task<string> SendRequestAsync(HttpRequest httpRequest);
	}
}
