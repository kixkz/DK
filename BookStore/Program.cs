using System.Text;
using BookStore.BL.BackgroundService;
using BookStore.BL.CommandHandlers;
using BookStore.DL.Repositories.MsSql;
using BookStore.Extensions;
using BookStore.HealthChecks;
using BookStore.Middleware;
using BookStore.Models.Configurations;
using BookStore.Models.Models.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);

builder.Services.Configure<MyJsonSettings>(
    builder.Configuration.GetSection(nameof(MyJsonSettings)));

// Add services to the container.
builder.Services.RegisterRepositories()
    .RegisterServices()
    .AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token in the textbox below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    x.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("View", policy =>
    {
        policy.RequireClaim("View");
    });
    option.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("Admin");
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.Configure<MyKafkaProducerSettings>(builder.Configuration.GetSection(nameof(MyKafkaProducerSettings)));
builder.Services.Configure<KafkaConsumerSettings>(builder.Configuration.GetSection(nameof(KafkaConsumerSettings)));

builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection(nameof(MongoDbConfiguration)));


builder.Services.AddHealthChecks()
    .AddCheck<SqlHealthCheck>("SQL Server")
    .AddUrlGroup(new Uri("Https://google.bg"), name: "Google Service")
    .AddCheck<CustomHealthCheck>("Server OK");

builder.Services.AddMediatR(typeof(GetAllBooksCommandHad).Assembly);

builder.Services.AddIdentity<User, UserRole>()
    .AddUserStore<UserInfoStore>()
    .AddRoleStore<UserRoleStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.MapHealthChecks("/health");

app.RegisterHealthChecks();

app.UseMiddleware<ErrorHandlerMiddleware>();

//app.UseMiddleware<CustomHandlerMiddleware>();

app.Run();
