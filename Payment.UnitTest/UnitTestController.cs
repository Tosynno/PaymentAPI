using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Newtonsoft.Json;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Utilities;
using PaymentAPI.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.UnitTest
{
    public class UnitTestController
    {
        private readonly Mock<ITransaction> _transaction;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        public UnitTestController()
        {
            _transaction = new Mock<ITransaction>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }
        [Fact]
        public async void NIPTransaction()
        {
            //arrange
            var response = new ApiResponseBase<object>();
            var niptran = NIPTransactionRequest();
            var req = JsonConvert.SerializeObject(niptran);
            var encrypt = Utils.EncryptString("zMdRgUkXp2s5v8y/B?O(H+MbPeShZxCe", req);
            //_transaction.Setup(x => x.NIPTransfer(niptran))
            //    .Returns(niptran);
          
            var transactionController = new TransactionController(_httpContextAccessor.Object, _transaction.Object);
            //act
            EncryptClass encryptClass = new EncryptClass();
            if (!string.IsNullOrEmpty(encrypt))
            {
                // Access properties or methods of myObject safely here.
                encryptClass.Data = encrypt;
                var Result = await transactionController.NIPTransfer(encryptClass);
                //assert
                var decrypt = Utils.DecryptString("zMdRgUkXp2s5v8y/B?O(H+MbPeShZxCe", Result.Value.Data.ToString());
                Assert.NotNull(Result);
                Assert.Equal(encrypt, Result.Result.ToString());
                Assert.Equal(decrypt, Result.ToString());
                Assert.True(niptran.Equals(decrypt));
            }
            else
            {
                Assert.NotNull("");
            }
           
        }

        private NIPTransactionRequest NIPTransactionRequest()
        {

            var NIPTransactionRequest = new NIPTransactionRequest
            {
                BankName = "Access Bank",
                BankCode = "0443",
                CreditMerchantNumber = "0078658767",
                DebitMerchantNumber = "0007854335",
                Narration = "Test",
                TransactionAmount = 100.90,
            };
             
            return NIPTransactionRequest;
        }
    }
}
