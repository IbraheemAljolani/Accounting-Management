using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using AccountingManagement.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AccountingManagementContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
#endregion

#region XML Comments
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Accounting Management",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion

#region Serilog
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
string loggerPath = configuration.GetSection("LoggerPath").Value;
Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).
                WriteTo.File(loggerPath, rollingInterval: RollingInterval.Day).
                CreateLogger();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
