namespace WemaAssessment.Application.DTOs
{
    public class CustomerResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public bool IsCustomerConfirmed { get; set; }
    }
}
