using Azure;
using Moq;
using PaymentAPI.Application.Dto;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Repositories;
using PaymentAPI.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace ApplicationLayerTest
{
    
    public class UnitTest1
    {
        public readonly ICustomerRepo _mockCustomerRepo;
        ApiResponseBase<object> _response;
        int _selector;
        //string res;
        public UnitTest1()
        {
            Mock<ICustomerRepo> mockCustomerRepo = new Mock<ICustomerRepo>();
            _response = new ApiResponseBase<object>();
            var metadat = new
            {
                AccountName = "",
                AccountNumber = "",
            };
            _response.Data = metadat;
            _response.ResponseCode = "00";
            _response.ResponseDescription = "SUCCESSFUL";

            mockCustomerRepo.Setup(CreateCustomer => CreateCustomer.CreateCustomer(It.IsAny<CreateCustomerRequest>()))
                .Returns((CreateCustomerRequest req) =>
                {
                    return new TaskFactory<ApiResponseBase<object>>().StartNew(()=>_response);
                });

            mockCustomerRepo.Setup(GetAllCustomer=>GetAllCustomer.GetAllCustomer(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<bool>(),It.IsAny<bool>())).
                Returns((int page, int pageSize, bool prev, bool next)=>
                {
                    ApiResponseBase<object> response = new();
                    return new TaskFactory<ApiResponseBase<object>>().StartNew(() =>
                    {
                        if (page == 2)
                        {
                            var metadata = new
                            {
                                Data = new List<CustomerDto>() {
                            new CustomerDto() {
                                Name = "TestDev",
                                NationalIDNumber = "12345",
                                Surname = "Agba Dev",
                                CustomerNumber = "12345",
                                DateofBirth = DateTime.UtcNow.AddYears(-26).ToString()
                            },
                            new CustomerDto() {
                                Name = "TestDev1",
                                NationalIDNumber = "123456",
                                Surname = "Agba Dev",
                                CustomerNumber = "123456",
                                DateofBirth = DateTime.UtcNow.AddYears(-27).ToString()
                            },
                            new CustomerDto() {
                                Name = "TestDev2",
                                NationalIDNumber = "1234567",
                                Surname = "Agba Dev",
                                CustomerNumber = "1234567",
                                DateofBirth = DateTime.UtcNow.AddYears(-28).ToString()
                            } }
                            };
                            response.Data = metadata;
                            response.ResponseCode = "01";
                            response.ResponseDescription = "UNSUCCESSFUL";
                        }
                        else if (page == 1)
                        {
                            var metadata = new
                            {
                                Data = new List<CustomerDto>() {
                            new CustomerDto() {
                                Name = "TestDev",
                                NationalIDNumber = "12345",
                                Surname = "Agba Dev",
                                CustomerNumber = "12345",
                                DateofBirth = DateTime.UtcNow.AddYears(-26).ToString()
                            }/*,
                            new CustomerDto() {
                                Name = "TestDev1",
                                NationalIDNumber = "123456",
                                Surname = "Agba Dev",
                                CustomerNumber = "123456",
                                DateofBirth = DateTime.UtcNow.AddYears(-27).ToString()
                            },
                            new CustomerDto() {
                                Name = "TestDev2",
                                NationalIDNumber = "1234567",
                                Surname = "Agba Dev",
                                CustomerNumber = "1234567",
                                DateofBirth = DateTime.UtcNow.AddYears(-28).ToString()
                            }*/
                            }
                            };
                            response.Data = metadata;
                            response.ResponseCode = "01";
                            response.ResponseDescription = "UNSUCCESSFUL";
                        }
                        else
                        {
                            if (page == 0) return response = null;
                            var metadata = new
                            {
                                Data = new List<CustomerDto>()
                                {
                                }
                            };
                            response.Data = metadata;
                            response.ResponseCode = "00";
                            response.ResponseDescription = "SUCCESSFUL";
                        }
                        return response;
                    });

                });

            _mockCustomerRepo = mockCustomerRepo.Object;
        }
        [Fact]
        public void GetCustomerTest()
        {
            ApiResponseBase<object> createCustomer = _mockCustomerRepo.CreateCustomer(new CreateCustomerRequest
            {
                  Name = "TestDev",
                  NationalIDNumber= "12345",
                  Surname="Dev",
                  CustomerNumber= "12345",
                  AccountNumber= "12345",
                  DateofBirth= DateTime.UtcNow.AddYears(-25)
            }).Result;

            Assert.NotNull(createCustomer); // Test if null
            Assert.IsType<ApiResponseBase<object>>(createCustomer); // Test type
            Assert.Equal("00",createCustomer.ResponseCode); // Verify it is the right 

        }

        [Fact]
        public void GetAllCustomerTest()
        {
            ApiResponseBase<object> allCustomer=_mockCustomerRepo.GetAllCustomer(2,1,false,false).Result;

            Assert.NotNull(allCustomer);
        }
    }
}