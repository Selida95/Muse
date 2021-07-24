using Moq;
using Moq.Protected;
using Muse.Models.Twitch;
using Muse.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Muse.Test
{
    public class TwitchClientTests
    {
        [Fact]
        public void GetAppAccessToken_ShouldRequestAndReturnsToken()
        {
            string mockAccessToken = GenerateAccessToken();
            int mockExpiryIn = GenerateExpiryInSeconds();
            TokenResponse tokenResponse = new TokenResponse
            {
                accessToken = mockAccessToken,
                expiresIn = mockExpiryIn,
                scope = new string[]
                {
                    "user:read:follows"
                },
                tokenType = "bearer"

            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(tokenResponse))
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var twitchClient = new TwitchClient(httpClient);

            var accessTokenResponse = twitchClient.GetAppAccessToken();

            Assert.Equal(tokenResponse, accessTokenResponse);
        }

        private string GenerateAccessToken()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 30)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        private int GenerateExpiryInSeconds()
        {
            return new Random().Next(4752000, 5616000);
        }
    }
}
