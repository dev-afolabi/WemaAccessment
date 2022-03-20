using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WemaAssessment.Application.Services.Interfaces
{
    public interface IOtpService
    {
        public string Otp { get; }
        string GenerateOtp();
    }
}
