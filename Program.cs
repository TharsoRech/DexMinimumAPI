using DexMinimumApi.Context;
using DexMinimumApi.Repository;
using DexMinimumApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IDexService, DexService>(); 
builder.Services.AddScoped<IDexRepository, DexRepository>(); 

app.UseHttpsRedirection();

app.MapPost("/vdi-dex", () =>
    {
         
    })
    .WithName("vdi-dex");

app.Run();