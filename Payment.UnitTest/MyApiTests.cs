using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Utilities;
using System.Text;

namespace Payment.UnitTest
{
    public class MyApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string baseUri;

        public MyApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            baseUri = "https://localhost:32768";
        }
    
        [Fact]
        public async Task createMarchant()
        {
            var prof = PaymentProfileReq();
            var req = JsonConvert.SerializeObject(prof);
            var encrypt = Utils.EncryptString("zMdRgUkXp2s5v8y/B?O(H+MbPeShZxCe", req);
            // Arrange
            var client = _factory.CreateClient();
            EncryptClass encryptClass = new EncryptClass();
            encryptClass.Data = encrypt;
            var buffer = System.Text.Encoding.UTF8.GetBytes(encryptClass.Data);
            var byteContent = new ByteArrayContent(buffer);
            // Act
            var response = await client.PostAsync($"{baseUri}/api/Account/createMarchant", byteContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the HTTP status code is 2xx.
        }

        [Fact]
        public async Task editMarchant()
        {
            var prof = UpdatePaymentProfile();
            var req = JsonConvert.SerializeObject(prof);
            var encrypt = Utils.EncryptString("zMdRgUkXp2s5v8y/B?O(H+MbPeShZxCe", req);
            // Arrange
            var client = _factory.CreateClient();
            EncryptClass encryptClass = new EncryptClass();
            encryptClass.Data = encrypt;
            var buffer = System.Text.Encoding.UTF8.GetBytes(encryptClass.Data);
            var byteContent = new ByteArrayContent(buffer);
            // Act
            var response = await client.PostAsync($"{baseUri}/api/Account/editMarchant", byteContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the HTTP status code is 2xx.
        }

        private PaymentProfileRequest PaymentProfileReq()
        {
            var PaymentProfileRequest = new PaymentProfileRequest
            {
                BusinessId = "08899857",
                BusinessName = "Castol NIG Ltd",
                ContactName = "Jame Paul",
                ContactSurname = "Walker",
                DateOfEstablishment = Convert.ToDateTime("08/09/2008"),
                //NationalIDNumber = "B099099",
                //Name = "Kingsley",
                //Surname = "Adewale",
            };

            return PaymentProfileRequest;
        }
        private UpdatePaymentProfileRequest UpdatePaymentProfile()
        {
            var PaymentProfileRequest = new UpdatePaymentProfileRequest
            {
                BusinessId = "08899857",
                BusinessName = "Castol NIG Ltd",
                ContactName = "Jame Paul",
                ContactSurname = "Walker",
                DateOfEstablishment = Convert.ToDateTime("08/09/2008"),
                NationalIDNumber = "B099099",
                Name = "Kingsley",
                Surname = "Adewale",
                MarchantNumber ="00909778866"
            };

            return PaymentProfileRequest;
        }
    }
}