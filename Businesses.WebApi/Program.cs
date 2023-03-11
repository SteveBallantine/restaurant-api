using Businesses.DataAccess;
using Businesses.DataAccess.Configuration;
using Businesses.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add our settings objects to the DI container.
builder.Services.AddSingleton(builder.Configuration.GetSection("YelpSettings").Get<YelpSettings>());
builder.Services.AddSingleton(builder.Configuration.GetSection("WebApiSettings").Get<WebApiSettings>());

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

app.UseAuthorization();

app.MapControllers();

app.Run();
