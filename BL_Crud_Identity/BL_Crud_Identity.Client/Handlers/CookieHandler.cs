using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BL_Crud_Identity.Client.Handlers
{
    /// <summary>
    /// Http message handler that forces HttpClient requests to include browser cookies for Identity authentication.
    /// </summary>
    public class CookieHandler : DelegatingHandler
    {
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Force the browser fetch mechanism to include credentials (cookies) for this request
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
