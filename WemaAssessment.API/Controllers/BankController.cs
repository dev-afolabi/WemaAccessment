using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WemaAssessment.Application.Services.Interfaces;

namespace WemaAssessment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        /// <summary>
        /// This endpoint returns list of banks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var response = await _bankService.GetAllBanksAsync();

            if (response?.Result.Count < 1) return NotFound();

            return Ok(response);
        }
    }
}
