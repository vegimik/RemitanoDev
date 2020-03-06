using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RemitanoDevTask.IntegrationTest.Infrastructure;
using Xunit;

namespace RemitanoDevTask.IntegrationTest
{
    public class MoviesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public MoviesControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        
        [Fact]
        public async Task Index_WhenCalled_ReturnsViewResult()
        {
            var response = await _client.GetAsync("/Movies/Index");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Enter your details below", responseString);//For this api should be authorized otherwise will route to Login-Form.
        }

      
    }
}
