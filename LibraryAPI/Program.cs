using ApplicationServices;
using LibraryAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.DbContexts;
using Persistence.Extensions;
using Shared.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Library Management APIs",
            Version = "1.0.0",
            Contact = new OpenApiContact { Email = "info@martha.com", Name = "Library Management" }
        }
    );
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
    c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .Build());
});

builder.Services.AddHttpContextAccessor();

// Register your DbContext with the DI container
builder.Services.AddApplicationServices().AddPersistence(builder.Configuration);

//Dependency Injection for Custom JWT authorization
builder.Services.AddCustomAuthorizationServices(builder.Configuration);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // Enforce lowercase URLs
    options.LowercaseQueryStrings = true; // Optional: lowercase query strings
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management v1");
        c.RoutePrefix = string.Empty;
    });
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
if (context.Database.IsSqlServer())
{
    await context.Database.MigrateAsync();
}
//Seed Books
try { await app.SeedBookData(); } catch (Exception) { }

app.UseCors("CorsPolicy");

//global error-handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();