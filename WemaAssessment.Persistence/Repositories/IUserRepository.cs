using System.Linq;
using WemaAssessment.Domain.Models;

namespace WemaAssessment.Persistence.Repositories
{
    public interface IUserRepository
    {
        IQueryable<Customer> GetAllCustomers();
    }
}
