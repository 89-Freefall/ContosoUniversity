using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IQuoteService _quoteService;

        public IndexModel(ILogger<IndexModel> logger, IQuoteService quoteService)
        {
            _logger = logger;
            _quoteService = quoteService;
        }

        public string Quote { get; private set; }

        public async Task OnGetAsync()
        {
            // Generate the quote
            Quote = await _quoteService.GenerateQuote();

            // Structured logging example
            _logger.LogInformation(
                "Index page loaded at {Time}, Quote length: {QuoteLength}",
                DateTime.UtcNow,
                Quote?.Length ?? 0
            );
        }
    }
}