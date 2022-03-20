using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WemaAssessment.Application.DTOs
{
    public class GetBankResponseDto
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("errorMessages")]
        public List<string> ErrorMessages { get; set; }
        [JsonProperty("hasError")]
        public bool HasError { get; set; }
        [JsonProperty("timeGenerated")]
        public DateTime TimeGenerated { get; set; }
    }

    public class Result
    {
        [JsonProperty("bankName")]
        public string BankName { get; set; }
        [JsonProperty("BankCode")]
        public string BankCode { get; set; }
    }
}