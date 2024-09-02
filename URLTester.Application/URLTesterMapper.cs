using AutoMapper;
using URLTester.Application.ViewModels;
using URLTester.Domain.Models;

namespace URLTester.Application;

public static class URLTesterMapper
{
    public static URLViewModel ToViewModel(this URL input, string baseURL)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<URL, URLViewModel>()
               .ForMember(dest => dest.Shortened, opt => opt.MapFrom(src => $"{baseURL}/{src.Shortened}"));
        });

        var mapper = new Mapper(config);
        return mapper.Map<URLViewModel>(input);
    }

    public static IReadOnlyList<URLViewModel> ToViewModel(this IReadOnlyList<URL> input, string baseURL)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<URL, URLViewModel>()
               .ForMember(dest => dest.Shortened, opt => opt.MapFrom(src => $"{baseURL}/{src.Shortened}"));
        });

        var mapper = new Mapper(config);
        return mapper.Map<IReadOnlyList<URLViewModel>>(input);
    }

}
