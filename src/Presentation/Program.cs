using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PennyPlannerDbContext>(opt => opt.UseInMemoryDatabase("PennyPlannerDbContext"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();