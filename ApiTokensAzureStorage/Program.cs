using ApiTokensAzureStorage.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<ServiceSaSToken>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();


//NECESITAMOS MAPEAR NUESTRO METODO token DENTRO DE
//UN ENDPOINT
app.MapGet("/token/{curso}", (string curso, ServiceSaSToken service) =>
{
    string token = service.GenerateToken(curso);
    return new { token = token };
});


app.Run();

