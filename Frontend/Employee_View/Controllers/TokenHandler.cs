using System.Net.Http.Headers;

namespace Employee_View.Controllers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            // token get for ...........
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(httpRequest, cancellationToken);
        }
    }
}
