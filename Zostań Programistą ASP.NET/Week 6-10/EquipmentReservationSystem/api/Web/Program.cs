using Application.Utils;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;
using System.Text.Json.Serialization;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentityCore<UserData>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<Context>()
                .AddApiEndpoints();

// Add logger
#pragma warning disable CS0618 // The component is outdated
Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MSSqlServerSinkOptions()
        {
            AutoCreateSqlDatabase = false,
            AutoCreateSqlTable = false,
            TableName = "Logs",
            SchemaName = "oss"
        })
    .CreateLogger();
#pragma warning restore CS0618 // The component is outdated
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
//builder.Host.UseSerilog();

// Add middleware
builder.Services.AddExceptionHandler<AppSystemExceptionMiddleware>();
builder.Services.AddExceptionHandler<GeneralExceptionMiddleware>();

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

// Authorize
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:JWT:Issuer"],
            ValidAudience = builder.Configuration["Token:JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:JWT:Key"]))
        };
    });
//builder.Services.AddAuthorization();

//Set Authorize Rule
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 5;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

//CORS
builder.Services.AddCors(options =>
{
    var allowedDomens = builder.Configuration["AllowedDomens"].Split(",");

    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedDomens)
        //builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowedToAllowWildcardSubdomains());
});

try
{
    Log.Information("Starting web application");

    var app = builder.Build();

    // request logging
    //app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(options =>
    {
        options.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    app.UseExceptionHandler(_ => { });
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}