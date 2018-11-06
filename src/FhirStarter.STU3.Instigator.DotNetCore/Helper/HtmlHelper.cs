using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace FhirStarter.STU3.Instigator.DotNetCore.Helper
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
