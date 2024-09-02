using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using URLTester.API.Configs;
using URLTester.Application;
using URLTester.Application.Behaviours;
using URLTester.Application.Configs;
using URLTester.Domain.Enums;
using URLTester.Domain.Repositories;
using URLTester.Infrastructure;
using URLTester.Infrastructure.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

builder.Services.AddControllers();

builder.Services.AddRazorPages();
builder.Services.AddLocalization();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddScoped<IURLRepository, URLRepository>();

// Add MiniProfiler services
builder.Services.AddMiniProfiler(options =>
{
	options.RouteBasePath = "/profiler"; // The route to view profiling results
	options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
	options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
}).AddEntityFramework(); // Add EF Core integration if needed
builder.Services.AddHttpClient("ProfiledHttpClient")
			.AddHttpMessageHandler(() => new ProfilingHttpMessageHandler());


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<URLTesterDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(URLTesterMapper));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});
builder.Services.AddValidatorsFromAssemblyContaining(typeof(URLTesterMapper));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URL Tester",
        Contact = new OpenApiContact
        {
            Name = "Hamza Mustafa",
            Url = new Uri("https://www.linkedin.com/in/hamza-mustafa-890ba9181/")
        }
    });

    c.OperationFilter<AddAcceptLanguageHeaderParameter>();

    c.EnableAnnotations();
});

builder.Services.AddHealthChecks().AddDbContextCheck<URLTesterDBContext>("Database HealthCheck");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<URLTesterDBContext>();
    dbContext.Database.Migrate();
}

var supportedCultures = Enum
    .GetValues(typeof(Languages))
    .Cast<Languages>()
    .Select(x => x.ToString())
    .ToArray();

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();


app.UseMiniProfiler();
app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();
app.MapHealthChecks("/healthz", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.Run();
