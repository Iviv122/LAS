using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LinkContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
    );

 builder.Services.AddCors(options => options.AddPolicy(
     "AllowAll",
     p => p.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader()));

var app = builder.Build();


app.UseCors("AllowAll");
app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LinkContext>();
    db.Database.EnsureCreated();
}

app.Run();

