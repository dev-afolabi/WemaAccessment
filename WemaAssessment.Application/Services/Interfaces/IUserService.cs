using System.Collections.Generic;
using System.Threading.Tasks;
using WemaAssessment.Application.DTOs;
using WemaAssessment.Application.Helpers;

namespace WemaAssessment.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<CustomerResponseDto>> RegisterCustomer(AddCustomerDto model);
        Task<Response<PaginatorHelper<IEnumerable<CustomerResponseDto>>>> GetCustomersAsync(int pageSize = 10, int pageNumber = 1);
        Task<Response<string>> ConfirmCustomer(string otp, string customerId);
    }
}
