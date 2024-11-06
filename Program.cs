using GradjevinskiMaterijali.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure the database context with the connection string from appsettings.json
builder.Services.AddDbContext<GradjevinskiMaterijaliContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GradjevinskiMaterijaliConnection")));

// Add controllers and configure Swagger for API documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS (optional, adjust as needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add JSON serializer options to handle cycles if needed
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();


// Enable serving static files from the wwwroot directory

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map controllers to API routes
app.MapControllers();

// Set fallback to index.html for unmatched routes
app.MapFallbackToFile("index.html");

app.Run();
