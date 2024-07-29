using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Business.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient with dependency injection
builder.Services.AddHttpClient();

// Register additional services with dependency injection
builder.Services.AddScoped<ISearchService, SearchService>();

// Configure logging
builder.Services.AddLogging(configure => configure.AddConsole());

// Add configuration services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    // Configure HSTS (HTTP Strict Transport Security) for production
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

await app.RunAsync();