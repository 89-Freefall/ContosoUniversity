using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Linq;
using ContosoUniversity.Data;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private DbConnection _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test"); // Use Test environment for integration tests

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SchoolContext>));
            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            // Remove existing DbConnection registration
            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection));
            if (dbConnectionDescriptor != null)
                services.Remove(dbConnectionDescriptor);

            // Register open SQLite in-memory connection
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            services.AddSingleton<DbConnection>(_connection);

            // Register DbContext to use in-memory SQLite
            services.AddDbContext<SchoolContext>((provider, options) =>
            {
                var connection = provider.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            // Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
            db.Database.EnsureCreated(); // <-- creates tables in memory, no migrations
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection?.Dispose();
        }
    }
}