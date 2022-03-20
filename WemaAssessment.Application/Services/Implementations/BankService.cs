using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using WemaAssessment.Application.DTOs;
using WemaAssessment.Application.Services.Interfaces;
using WemaAssessment.Application.Settings;

namespace WemaAssessment.Application.Services.Implementations
{
    public class BankService : IBankService
    {
        private readonly RestClient _client;
        private readonly BankSettings _bankSettings;

        public BankService(IOptions<BankSettings> bankSettings)
        {
            _bankSettings = bankSettings.Value;
            _client = new RestClient(_bankSettings.BaseUrl);
        }

        public async Task<GetBankResponseDto> GetAllBanksAsync()
        {
            var request = new RestRequest(_bankSettings.Resource, Method.Get);
            request.AddHeader("Ocp-Apim-Subscription-Key", _bankSettings.ConfigurationKey);
            var response = await _client.ExecuteAsync<GetBankResponseDto>(request);

            return response.Data;
        }
    }
}
