using System.Threading.Tasks;
using WemaAssessment.Application.DTOs;

namespace WemaAssessment.Application.Services.Interfaces
{
    public interface IBankService
    {
        Task<GetBankResponseDto> GetAllBanksAsync();
    }
}
