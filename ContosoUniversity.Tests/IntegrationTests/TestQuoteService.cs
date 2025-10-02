using AngleSharp.Html.Dom;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.Tests.IntegrationTests
{
    // Mock service
    public class TestQuoteService : IQuoteService
    {
        public Task<string> GenerateQuote()
        {
            return Task.FromResult(
                "Something's interfering with time, Mr. Scarman, " +
                "and time is my business.");
        }
    }

    // Test class
    public class IndexPageTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IndexPageTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_QuoteService_ProvidesQuoteInPage()
        {
            // Arrange: inject mock service
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IQuoteService, TestQuoteService>();
                });
            })
            .CreateClient();

            // Act
            var defaultPage = await client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var quoteElement = content.QuerySelector("#quote");

        }
    }
}