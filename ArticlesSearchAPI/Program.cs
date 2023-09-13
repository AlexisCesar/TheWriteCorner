using log4net;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ArticlesSearchDatabaseSettings>(
    builder.Configuration.GetSection("ArticlesSearchDatabase")
);

builder.Services.AddSingleton<IArticlesService, ArticlesService>();

builder.Logging.AddLog4Net();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var logger = LogManager.GetLogger(typeof(Program));

app.MapGet("/api/v1/articles", async (IArticlesService service) =>
{
    try
    {
        return Results.Ok(await service.GetAsync());
    } catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Problem("Something went wrong with our database, please try again later.");
    }
});

app.MapGet("/api/v1/articles/{id}", async (IArticlesService service, [FromQuery] string id) =>
{
    try
    {
        var result = await service.GetAsync(id);
        return result != null ? Results.Ok(result) : Results.NotFound("We could not find this article in our database");
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Problem("Something went wrong with our database, please try again later.");
    }
});

app.Run();