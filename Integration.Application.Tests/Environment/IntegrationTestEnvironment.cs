using System.Net.Http.Json;
using Lacuna.Application.DTO;
using Lacuna.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Integration.Application.Tests.Environment;

public class IntegrationTestEnvironment
{
    protected HttpClient GenerateClient()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<LacunaContext>));

                    var dbName = Guid.NewGuid().ToString();
                    services.AddDbContext<LacunaContext>(opt =>
                    {
                        var dbContextOptions = opt.UseInMemoryDatabase(dbName).Options;
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<LacunaContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<Program>>();

                        db.Database.EnsureCreated();

                        try
                        {
                            Utilities.InitializeDatabase(db);
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, "An error occurred seeding the database with test messages. Error: {Message}", e.Message);
                        }
                    }
                    
                });
            });

        var client = application.CreateClient();

        return client;
    }

    protected async Task<HttpResponseMessage> Register(HttpClient client, CreateUserDto registerDto)
    {
        var response = await client.PostAsJsonAsync("api/Users/create", registerDto);
        return response;
    }

    protected async Task<HttpResponseMessage> Login(HttpClient client, LoginUserDto loginDto)
    {
        var response = await client.PostAsJsonAsync("api/Users/login", loginDto);
        return response;
    }
}