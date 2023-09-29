using BookSearchAPI;
using BookSearchAPI.Services;
using BookSearchAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BookDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddScoped<IModelInitService, ModelInitService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddPredictionEnginePool<BookRating, BookRatingPrediction>().FromFile(builder.Configuration["MLModelPath"]);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
