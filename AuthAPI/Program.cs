using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.RabbitMq;
using AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connString = builder.Configuration.GetConnectionString("UserConnection");

builder.Services.AddDbContext<UserDbContext>
    (opts =>
    {
        opts.UseMySql(connString, ServerVersion.AutoDetect(connString));
    });

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

var app = builder.Build();

// Run migrations if needed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
