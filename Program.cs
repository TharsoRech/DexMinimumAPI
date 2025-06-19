using DexMinimumApi.Context;
using DexMinimumApi.Models.Dtos;
using DexMinimumApi.Repository;
using DexMinimumApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IDexService, DexService>();
builder.Services.AddScoped<IDexRepository, DexRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/vdi-dex", async (
    HttpContext context,
    IConfiguration configuration,
    IDexService dexService) =>
{
    var authHeader = context.Request.Headers["Authorization"].ToString();
    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
    {
        context.Response.Headers["WWW-Authenticate"] = "Basic";
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return Results.StatusCode(401);
    }

    var encodedCredentials = authHeader["Basic ".Length..].Trim();
    var credentialBytes = Convert.FromBase64String(encodedCredentials);
    var credentials = System.Text.Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

    var username = credentials[0];
    var password = credentials[1];

    var userInSettings = configuration.GetConnectionString("DexUser");
    var passwordInSettings = configuration.GetConnectionString("DexPassword");
    // Validate credentials
    if (username != userInSettings || password != passwordInSettings)
    {
        context.Response.Headers["WWW-Authenticate"] = "Basic";
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return Results.StatusCode(403);
    }

    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

    var payload = System.Text.Json.JsonSerializer.Deserialize<DexPayload>(requestBody);

    if (payload != null)
    {
        // Save to database via injected service
        await dexService.SaveDexFile(payload.DexFileContent, payload.MachineName);
    }
    else
    {
        return Results.UnprocessableEntity();
    }

    return Results.Ok(new
    {
        Message = $"Received DEX from {payload.MachineName}",
        Size = payload.DexFileContent.Length
    });
});

app.Run();