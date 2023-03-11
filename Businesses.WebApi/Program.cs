using Businesses.DataAccess;
using Businesses.DataAccess.Configuration;
using Businesses.WebApi.Configuration;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add our settings objects to the DI container.
builder.Services.AddSingleton(builder.Configuration.GetSection("YelpSettings").Get<YelpSettings>());
var apiSettings = builder.Configuration.GetSection("WebApiSettings").Get<WebApiSettings>();
builder.Services.AddSingleton(apiSettings);

// Add a response cache profile
builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add(Businesses.WebApi.Constants.ApiCacheProfileName,
        new CacheProfile()
        {
            Duration = apiSettings.ResponseCacheSeconds,
            Location = ResponseCacheLocation.Any,
            NoStore = false
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add an HttpClient instance
builder.Services.AddSingleton<HttpClient>(new HttpClient());
// Use the YelpBusinessDataService
builder.Services.AddSingleton<IBusinessDataService, YelpBusinessDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
