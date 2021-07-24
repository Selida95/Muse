using Microsoft.Extensions.Options;
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
        public async void GetAppAccessToken_ShouldRequestAndReturnsToken()
        {
            TokenResponse token = new TokenResponse
            {
                accessToken = GenerateRandomString(),
                expiresIn = GenerateExpiryInSeconds(),
                scope = new string[]
                {
                    "user:read:follows"
                },
                tokenType = "bearer"

            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(token))
            };

            TwitchSettings settings = new TwitchSettings
            {
                clientId = GenerateRandomString(),
                clientSecret = GenerateRandomString(),
                scope = new string[]
    {
                    "user:read:follows"
    }
            };

            // Mock http response
            Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
            // Mock settings
            Mock<IOptions<TwitchSettings>> mockSettings = new Mock<IOptions<TwitchSettings>>();

            mockSettings.Setup(c => c.Value).Returns(settings);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            //var mockSettings = Options.Create(settings);

            TwitchClient twitchClient = new TwitchClient(httpClient, mockSettings.Object);
            TokenResponse accessTokenResponse = await twitchClient.GetAppAccessToken();

            Assert.Equal(token.accessToken, accessTokenResponse.accessToken);
        }



        private string GenerateRandomString()
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
