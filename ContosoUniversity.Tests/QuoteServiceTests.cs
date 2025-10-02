using System.Threading.Tasks;
using ContosoUniversity.Services; // reference your main project
using Xunit;

namespace ContosoUniversity.Tests
{
    public class QuoteServiceTests
    {
        private readonly QuoteService _service;

        public QuoteServiceTests()
        {
            _service = new QuoteService();
        }

        [Fact]
        public async Task GenerateQuote_ReturnsNonEmptyString()
        {
            var result = await _service.GenerateQuote();
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public async Task GenerateQuote_ReturnsExpectedQuote()
        {
            var result = await _service.GenerateQuote();
            Assert.Contains("Come on, Sarah", result); // checks a substring
        }
    }
}