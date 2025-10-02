using System.Threading.Tasks;

namespace ContosoUniversity.Services
{
    public interface IQuoteService
    {
        Task<string> GenerateQuote();
    }
}