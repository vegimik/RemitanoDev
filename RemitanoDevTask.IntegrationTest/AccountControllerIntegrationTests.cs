using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RemitanoDevTask.IntegrationTest.Infrastructure;
using RemitanoDevTask.ViewModels;
using RestSharp;
using Xunit;

namespace RemitanoDevTask.IntegrationTest
{
    public class AccountControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AccountControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task LogIn_WhenPOSTExecuted_ReturnsToIndexView()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Account/Login");

            var formModel = new Dictionary<string, string>
            {
                { "UserName", "remitanodev@gmail.com" },
                { "Password", "Remitano1+" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("This user doesn&#x27;t exist.", responseString);
        }


        [Fact]
        public async Task Create_WhenCalled_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("/Account/Register");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Create a new account!", responseString);
        }

        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Account/Register");

            var formModel = new Dictionary<string, string>
            {
                { "UserName", "username_shouldBe_emailDataType" },
                { "Password", "password_shouldBe_passwordDataType" },
                { "Lloji", "lloji" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("Something went wrong.", responseString);

        }

        [Fact]
        public async Task Create_WhenPOSTExecuted_ReturnsToIndexView()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/Account/Register");

            var formModel = new Dictionary<string, string>
            {
                { "UserName", "remitanodev@gmail.com" },
                { "Password", "remitano1+" },
                { "Lloji", "0" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Something went wrong.", responseString);

        }

    }
}
