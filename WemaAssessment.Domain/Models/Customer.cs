using Microsoft.AspNetCore.Identity;

namespace WemaAssessment.Domain.Models
{
    public class Customer : IdentityUser
    {
        public string State { get; set; }
        public string LGA { get; set; }
    }
}
