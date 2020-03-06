using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RemitanoDevTask.IntegrationTest.Infrastructure;
using Xunit;

namespace RemitanoDevTask.IntegrationTest
{
    public class HomeControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsViewResult()
        {
            var response = await _client.GetAsync("/");//That means "Home/Index"

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("REMITANO - How Does It Work?", responseString);
        }




    }
}
