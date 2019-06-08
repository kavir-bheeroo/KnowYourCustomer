using IdentityModel.Client;
using KnowYourCustomer.Common.Http.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace KnowYourCustomer.Common.Http.Clients
{
    /// <summary>
    /// Followed Identity Server OAuth token client from - https://blog.joaograssi.com/typed-httpclient-with-messagehandler-getting-accesstokens-from-identityserver/
    /// </summary>
    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly HttpClient _httpClient;
        private readonly ClientCredentialsTokenRequest _tokenRequest;
        private readonly ILogger<IdentityServerClient> _logger;

        public IdentityServerClient(
            HttpClient httpClient,
            ClientCredentialsTokenRequest tokenRequest,
            ILogger<IdentityServerClient> logger)
        {
            _httpClient = Guard.IsNotNull(httpClient, nameof(httpClient));
            _tokenRequest = Guard.IsNotNull(tokenRequest, nameof(tokenRequest));
            _logger = Guard.IsNotNull(logger, nameof(logger));
        }

        public async Task<string> RequestClientCredentialsTokenAsync()
        {
            // request the access token token
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);
            if (tokenResponse.IsError)
            {
                _logger.LogError(tokenResponse.Error);
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }
            return tokenResponse.AccessToken;
        }
    }
}