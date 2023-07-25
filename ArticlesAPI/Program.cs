using ArticlesAPI.DTOs.Command;
using ArticlesAPI.Mappings;
using ArticlesAPI.Models;
using ArticlesAPI.RabbitMq;
using ArticlesAPI.Services;
using ArticlesAPI.Validators;
using AutoMapper;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ArticlesManagementDatabaseSettings>(
    builder.Configuration.GetSection("ArticlesManagementDatabase")
);

builder.Services.AddSingleton<IArticlesService, ArticlesService>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

// Validators
builder.Services.AddScoped<IValidator<Article>, ArticleValidator>();
builder.Services.AddScoped<IValidator<Comment>, CommentValidator>();

// Mapping
var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

// Logger
builder.Logging.AddLog4Net();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
