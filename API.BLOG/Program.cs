using APP.BLOG.Models;
using Microsoft.EntityFrameworkCore;
using APP.BLOG.Features;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BlogDb");
builder.Services.AddDbContext<ProjectsDb>(options => options.UseSqlServer(connectionString));  // for IMediator injection in controllers

builder.Services.AddControllers();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BlogDbHandler).Assembly)); // for IMediator injection in controllers


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

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
