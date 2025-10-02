using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    // Apply authorization convention to SecurePage 
    options.Conventions.AuthorizePage("/SecurePage");
});

// Register QuoteService
builder.Services.AddScoped<IQuoteService, QuoteService>();

// Only register SQL Server if not running integration tests
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")
            ?? throw new InvalidOperationException("Connection string 'SchoolContext' not found.")));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

// Seed database only outside integration tests
if (!builder.Environment.IsEnvironment("Test"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<SchoolContext>();

        // Apply migrations and seed database
        context.Database.Migrate();
        DbInitializer.Initialize(context);

        var xmlFile = Path.Combine(app.Environment.ContentRootPath, "Data", "SeedData.xml");
        Console.WriteLine("Seeding database from SeedData.xml...");
        DbInitializer.SeedCoursesFromXml(context, xmlFile);
        Console.WriteLine("Database seeding completed.");
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();

// Make Program partial for integration testing
public partial class Program { }