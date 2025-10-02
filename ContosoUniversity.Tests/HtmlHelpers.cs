using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using System.Net.Http;
using System.Threading.Tasks;

public static class HtmlHelpers
{
    public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var html = await response.Content.ReadAsStringAsync();
        var parser = new HtmlParser();
        return parser.ParseDocument(html);
    }
}