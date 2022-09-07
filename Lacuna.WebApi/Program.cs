using Lacuna.Domain.Interfaces;
using Lacuna.Infrastructure.Context;
using Lacuna.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

if (builder.Environment.IsProduction())
{
    var connectionString = builder.Configuration.GetConnectionString("ProdConnection");
    builder.Services.AddDbContext<LacunaContext>(opt => 
        opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

builder.Services.AddDbContext<LacunaContext>(opt => opt.UseInMemoryDatabase("DevelopmentDatabase"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<LacunaContext>();
        db.Database.Migrate();
    }
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }