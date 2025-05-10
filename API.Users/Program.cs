using Microsoft.EntityFrameworkCore;
using APP.Users.Domain;
using APP.Users.Features;
using APP.Users;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// IOC:
var connectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UsersDb>(options => options.UseSqlServer(connectionString));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UsersDbHandler).Assembly));

// AppSettings:
//var section = builder.Configuration.GetSection(nameof(AppSettings));
//section.Bind(new AppSettings());

builder.Services.AddControllers();
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
