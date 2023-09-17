using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Payment.UnitTest
{
    public class MyApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MyApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
    
        [Fact]
        public async Task Test1()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/myendpoint");

            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the HTTP status code is 2xx.
        }
    }
}