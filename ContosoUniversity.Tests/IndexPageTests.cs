using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

public class IndexPageTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public IndexPageTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    // Integration test: check search form functionality
    [Fact]
    public async Task Get_SearchForm_ReturnsResults()
    {
        // Arrange: Reinitialize in-memory DB with test data
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
            Utilities.ReinitializeDbForTests(db);

            if (!db.Students.Any())
            {
                db.Students.Add(new Student { FirstMidName = "Test", LastName = "Student" });
                db.SaveChanges();
            }
        }

        // Act: Request the Students/Index page
        var defaultPage = await _client.GetAsync("/Students/Index");
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

        // Find the search form
        var form = content.QuerySelector("form") as IHtmlFormElement;
        Assert.NotNull(form);

        var button = form.QuerySelector("input[type='submit']") as IHtmlInputElement;
        Assert.NotNull(button);

        // Perform a search
        var response = await _client.GetAsync("/Students/Index?searchString=Test");

        // Assert: Both page loads succeed
        Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Integration test: check that QuoteService value appears in page
    [Fact]
    public async Task Get_IndexPage_ReturnsQuote()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Find the hidden input for quote
        var quoteElement = content.QuerySelector("#quote") as IHtmlInputElement;

        // Assert: quote exists and is not empty
        Assert.NotNull(quoteElement);
        Assert.False(string.IsNullOrWhiteSpace(quoteElement.Value));
    }
}