using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Repositories;
using PaymentAPI.Application.Utilities;

namespace PaymentAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected ITransaction _transaction;


        public TransactionController(IHttpContextAccessor httpContextAccessor, ITransaction transaction)
        {
            _httpContextAccessor = httpContextAccessor;
            _transaction = transaction;
        }

        [HttpPost("intraBankTransfer")]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> IntraBankTransfer(EncryptClass data)
        {
            var res = new ApiResponseNoData();
            var reslt = _httpContextAccessor.HttpContext?.Items?["data"]?.ToString();
            var splitRes = reslt?.Split('=');
            if (splitRes != null && splitRes[0].Equals("Invalid client"))
            {
                res.ResponseCode = "03";
                res.ResponseDescription = "Invalid client";
                return BadRequest(res);
            }
            var deserializeReq = JsonConvert.DeserializeObject<IntraBankTransferRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _transaction.IntraBankTransfer(deserializeReq);
            if (result.ResponseCode == "00")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost("nipTransfer")]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> NIPTransfer(EncryptClass data)
        {
            var res = new ApiResponseNoData();
            var reslt = _httpContextAccessor.HttpContext?.Items?["data"]?.ToString();
            var splitRes = reslt?.Split('=');
            if (splitRes != null && splitRes[0].Equals("Invalid client"))
            {
                res.ResponseCode = "03";
                res.ResponseDescription = "Invalid client";
                return BadRequest(res);
            }
            var deserializeReq = JsonConvert.DeserializeObject<NIPTransactionRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _transaction.NIPTransfer(deserializeReq);
            if (result.ResponseCode == "00")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("verifyAccount")]
        // [ValidateAuthRequestAttribute]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> VerifyAccount(string data)
        {
            var res = new ApiResponseNoData();
            var reslt = _httpContextAccessor.HttpContext?.Items?["data"]?.ToString();
            var splitRes = reslt?.Split('=');
            if (splitRes != null && splitRes[0].Equals("Invalid client"))
            {
                res.ResponseCode = "03";
                res.ResponseDescription = "Invalid client";
                return BadRequest(res);
            }
            var deserializeReq = splitRes[^1].Split(",");
            if (deserializeReq.ToString() == null)
            {
                res.ResponseCode = "03";
                res.ResponseDescription = "Invalid request";
                return BadRequest(res);
            }
            var accountnumber = deserializeReq[0].Trim();
            var result = await _transaction.VerifyAccount(accountnumber);
            return Ok(result);
        }
    }
}
