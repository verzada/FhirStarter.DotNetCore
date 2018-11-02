using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace FhirStarter.Inferno.WebAPI.Config
{
    public static class HtmlHelper
    {
        public static string GetDisplayUrlFromRequest(HttpRequest request)
        {
            var displayUrl = request.GetDisplayUrl();
            return displayUrl;
        }
    }
}
