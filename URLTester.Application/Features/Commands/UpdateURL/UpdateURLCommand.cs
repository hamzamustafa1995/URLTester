using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLTester.Application.ViewModels;

namespace URLTester.Application.Features.Commands.UpdateURL
{
	public record UpdateURLCommand : IRequest<ResultViewModel<bool>>
	{
		public int UrlId { get; set; }

		public string Url { get; set; }
		public UpdateURLCommand(string Url)
		{
			this.Url = Url;
		}
	}
}
