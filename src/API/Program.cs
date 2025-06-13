using Microsoft.EntityFrameworkCore;
using Services;
using Services.DbContext;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PersonalDatabase") ?? 
                       throw new InvalidOperationException("Personal connection string not found");
builder.Services.AddDbContext<DriverDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IDriverService, DriverService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();