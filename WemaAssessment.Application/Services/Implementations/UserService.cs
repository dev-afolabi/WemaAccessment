using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WemaAssessment.Application.DTOs;
using WemaAssessment.Application.Helpers;
using WemaAssessment.Application.Services.Interfaces;
using WemaAssessment.Domain.Models;
using WemaAssessment.Persistence.Repositories;

namespace WemaAssessment.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public UserService(UserManager<Customer> userManager, IMapper mapper, IUserRepository userRepository, IOtpService otpService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public async Task<Response<string>> ConfirmCustomer(string otp, string customerId)
        {
            var user = await _userManager.FindByIdAsync(customerId);
            if (user == null) return Response<string>.Fail($"User with id: {customerId} not found.", 404);

            if(user.PhoneNumberConfirmed) return Response<string>.Fail($"Customer already confirmed.", 400);

            //This would be assumed to be gotten via sms
            if (otp != _otpService.Otp) return Response<string>.Fail("Confirming user phone number failed, Invalid OTP provided", 400);

            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            if (string.IsNullOrWhiteSpace(token)) return Response<string>.Fail("Confirming user phone number failed", 400);

            var result = await _userManager
                .ChangePhoneNumberAsync(user, user.PhoneNumber, token);
            if(!result.Succeeded) return Response<string>.Fail("Confirming user phone number failed", 400);

            return Response<string>.Success("User confirmed successfully",null, 200);
        }

        public async Task<Response<PaginatorHelper<IEnumerable<CustomerResponseDto>>>> GetCustomersAsync(int pageSize = 10, int pageNumber = 1)
        {
            var response =  _userRepository.GetAllCustomers();
            if (response == null) return Response<PaginatorHelper<IEnumerable<CustomerResponseDto>>>.Fail("No customer found", 404);

            var pagedResponse = await Paginator.PaginateAsync<Customer, CustomerResponseDto>(response, pageSize, pageNumber, _mapper);

            return Response<PaginatorHelper<IEnumerable<CustomerResponseDto>>>.Success("Customers retrieved successfully", pagedResponse, 200);
        }

        public async Task<Response<CustomerResponseDto>> RegisterCustomer(AddCustomerDto model)
        {
            var cutomerToAdd = _mapper.Map<Customer>(model);
            cutomerToAdd.UserName = model.Email;
            var response = await _userManager.CreateAsync(cutomerToAdd);

            if (!response.Succeeded) return Response<CustomerResponseDto>.Fail($"{response.Errors.First()}", 400);

            var otp = _otpService.GenerateOtp();
            //Send token to customer via email or sms
            //Emailsende.SendOtp(otp)

            var returnedUser = await _userManager.FindByEmailAsync(cutomerToAdd.Email);
            var mappedResponse = _mapper.Map<CustomerResponseDto>(returnedUser);

            return Response<CustomerResponseDto>.Success($"Customer created successfully",mappedResponse, 200);
        }
    }
}
