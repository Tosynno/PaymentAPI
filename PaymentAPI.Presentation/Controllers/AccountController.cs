using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Utilities;

namespace PaymentAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected IMarchantProfile _marchantProfile;
        protected ICustomerRepo _customerRepo;

        public AccountController(IHttpContextAccessor httpContextAccessor, IMarchantProfile marchantProfile, ICustomerRepo customerRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _marchantProfile = marchantProfile;
            _customerRepo = customerRepo;
        }

        [HttpPost("createMarchant")]
       [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> CreateMarchant(EncryptClass data)
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
            var deserializeReq = JsonConvert.DeserializeObject<PaymentProfileRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _marchantProfile.CreateMarchant(deserializeReq);
            if (result.ResponseCode == "00")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("editMarchant")]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> EditMarchant(EncryptClass data)
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
            var deserializeReq = JsonConvert.DeserializeObject<UpdatePaymentProfileRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _marchantProfile.EditMarchant(deserializeReq);
            if (result.ResponseCode == "00")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("getAllMarchant")]
       // [ValidateAuthRequestAttribute]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> GetAllMarchant(string data)
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
            var deserializeReq = JsonConvert.DeserializeObject<GetAllMarchantRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _marchantProfile.GetAllMarchant(deserializeReq.pageIndex, deserializeReq.pageSize, deserializeReq.previous, deserializeReq.next);
            return Ok(result);
        }

        [HttpGet("setTransactionLimit")]
        // [ValidateAuthRequestAttribute]
        [ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> SetAverageTransaction(string data)
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
            var deserializeReq = JsonConvert.DeserializeObject<AverageTransactionRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _marchantProfile.SetAverageTransaction(deserializeReq);
            return Ok(result);
        }

        [HttpPost("createCustomer")]
        //[ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult<ApiResponseNoData>> CreateCustomer(CreateCustomerRequest data)
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
            var deserializeReq = JsonConvert.DeserializeObject<CreateCustomerRequest>(splitRes[^1]);
            if (deserializeReq == null)
            {
                var response = new ApiResponseNoData() { ResponseCode = "30", ResponseDescription = "invalid request" };
                return BadRequest(response);
            }
            var result = await _customerRepo.CreateCustomer(data);
            if (result.ResponseCode == "00")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
