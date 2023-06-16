global using Data;
global using Microsoft.EntityFrameworkCore;

using Api;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var provider = configuration.GetValue("Provider", "sqlite");
builder.Services.AddDbContextFactory<SampleDbContext>(options => options.UseDatabase(configuration, provider));
//builder.Services.AddDbContextFactory<SampleDbContext>(options =>
//    options.UseNpgsql(configuration.GetConnectionString("NPGSQLConnection")));

builder.Services.AddHostedService<Worker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

