using Employee_View.Models;
using System.Net.Http.Headers;

namespace Employee_View.Controllers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public TokenHandler(IHttpContextAccessor httpContextAccessor
                               , IHttpClientFactory factory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = factory.CreateClient("RefreshToken");
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
            var response = await base.SendAsync(httpRequest, cancellationToken);
            // return response;

            // if current token expire then generate refreshh token start

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var refreshToken = _httpContextAccessor.HttpContext.Session.GetString("RefreshToken");
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return response;
                }
                var dto = new RefrenceTokenDTO
                {
                    RefreshToken = refreshToken
                };              
                var responseRequest = await _httpClient.PostAsJsonAsync("Api/UserAPI/RefreshToken", dto);
                if (!responseRequest.IsSuccessStatusCode)
                {
                    throw new UnauthorizedAccessException("sorry please again login");
                }
                if (responseRequest.IsSuccessStatusCode)
                {
                    var read = await responseRequest.Content.ReadFromJsonAsync<RefrenceTokenDTO>();
                    _httpContextAccessor.HttpContext.Session.SetString("RefreshToken", read.RefreshToken);
                    _httpContextAccessor.HttpContext.Session.SetString("JWTToken", read.Token);
                    httpRequest.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", read.Token);

                    response = await base.SendAsync(httpRequest, cancellationToken);
                }
            }
            // if current token expire then generate refreshh token end

            return response;
        }
    }
}
