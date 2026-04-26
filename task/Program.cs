using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using task;
using task.Database;
using task.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")
    ?? throw new InvalidOperationException("Database connection string not found");

builder.Services.AddDbContext<DellinDictionaryDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<ImportService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.MapControllers(); // No authorization needed
app.Run();