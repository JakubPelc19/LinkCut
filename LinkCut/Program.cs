using LinkCut.Data;
using LinkCut.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddCors(options =>
{
    try
    {


        string? allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins");

        string[]? allowedOriginsArray = allowedOrigins?.Split(";");
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins(allowedOriginsArray).WithHeaders("Content-Type").WithMethods("GET", "POST");
            });
    }
    catch
    {
        throw new Exception("Couldn't find AllowedOrigins in appsettings.json");
    }
});
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILinkCutterService, LinkCutterService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
