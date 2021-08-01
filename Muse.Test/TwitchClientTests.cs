using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Muse.Data;
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
        public async void RequestAppAccessToken_ShouldRequestAndReturnsToken()
        {
            TokenResponse token = new TokenResponse
            {
                AccessToken = GenerateRandomString(),
                ExpiresIn = GenerateExpiryInSeconds(),
                Scope = new string[]
                {
                    "user:read:follows"
                },
                TokenType = "bearer"

            };

            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(token))
            };

            TwitchSettings settings = new TwitchSettings
            {
                ClientId = GenerateRandomString(),
                ClientSecret = GenerateRandomString(),
                Scope = new string[]
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

            TwitchClient twitchClient = new TwitchClient(httpClient, mockSettings.Object, null);
            TokenResponse accessTokenResponse = await twitchClient.RequestAppAccessToken();

            Assert.Equal(token.AccessToken, accessTokenResponse.AccessToken);
        }

        [Fact]
        public async Task SaveAccessToken_ShouldSaveAccessTokenInDatabase()
        {
            string randomToken = GenerateRandomString();
            int expiryTime = GenerateExpiryInSeconds();
            TokenResponse token = new TokenResponse
            {
                AccessToken = randomToken,
                ExpiresIn = expiryTime,
                Scope = new string[]
                {
                    "user:read:follows",
                    "user:read:subscriptions"
                },
                TokenType = "bearer"

            };

            IDbContextFactory<DataContext> contextFactory = TestData.GetDbContextFactory();
            using DataContext context = contextFactory.CreateDbContext();

            TwitchClient twitchClient = new TwitchClient(null, null, contextFactory);

            await twitchClient.SaveAccessToken(token);

            string databaseToken = context.TwitchTokens.FirstOrDefault(t => t.AccessToken == randomToken).AccessToken;
            Assert.Equal(databaseToken, randomToken);
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
