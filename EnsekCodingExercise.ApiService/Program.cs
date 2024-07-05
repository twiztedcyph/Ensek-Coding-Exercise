using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Services;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// I'm using sql server here because I have it setup. Any database compatible with EF would work.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<CustomerContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<IReadingsService, ReadingsService>();

// Add the ability to version our API.
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.HttpMethod}_{e.RelativePath}");
    options.UseInlineDefinitionsForEnums();
    options.DescribeAllParametersInCamelCase();
    options.CustomSchemaIds(x => x.FullName);
    options.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("00:00:00")
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// I've not been asked to add authorization or authentication so I'm not going to. But normally I would setup it up in here.

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "/v{version:apiVersion}/{controller}/{action}"
);

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

await app.RunAsync();
