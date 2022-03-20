using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WemaAssessment.API.Mappings;
using WemaAssessment.Application.Services.Interfaces;
using WemaAssessment.Domain.Models;
using WemaAssessment.Persistence;

namespace WemaAssessment.Tests.UserServiceTests
{
    class UserServiceTests
    {
        private static string path = Path.GetFullPath(@"../../../UserServiceTests/");

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        ApplicationDbContext context;
        IQueryable<Customer> customerList = Enumerable.Empty<Customer>().AsQueryable();
        private static IUserService _userService;
        private static IMapper _mapper;

        [SetUp]
        public void setup()
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase("Data Source=:memory:");
            context = new ApplicationDbContext(optionsBuilder.Options);

            if (context.Customers.Count() < 1)
            {
                foreach (var item in GetSampleData<Customer>(File.ReadAllText(path + "customers.json")))
                {
                    context.Add(item);
                    customerList.Append(item);
                }
                context.SaveChanges();
            }

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfiles());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

        }


        [Test]
        public async Task RegisterCustomer_whenCalled_withValidInputs_Pass()
        {
            //Arrange
            using (context)
            {
                var customer = new Customer
                {
                    Email = "liljosh@gmail.com",
                    UserName = "liljosh@gmail.com",
                    PhoneNumber = "08066745642",
                    State = "Oyo state",
                    LGA = "Iseyin"
                };
                var mockStore = new Mock<IUserStore<Customer>>();

                var _userMgr = new Mock<UserManager<Customer>>(mockStore.Object, null, null, null, null, null, null, null, null);
                _userMgr.Setup(x => x.CreateAsync(customer, "P@ssword"))
                            .Returns(Task.FromResult(IdentityResult.Success));

                _userMgr.Setup(x => x.FindByEmailAsync(customer.Email))
                .Returns(Task.FromResult(customer));

                //Act
                Task<IdentityResult> tt = (Task<IdentityResult>)_userMgr.Object.CreateAsync(customer, "P@ssword");
                var user = await _userMgr.Object.FindByEmailAsync("liljosh@gmail.com");

                //Assert
                Assert.AreEqual("liljosh@gmail.com", user.Email);
            }
        }

        private static List<T> GetSampleData<T>(string location)
        {
            var output = JsonSerializer.Deserialize<List<T>>(location, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }
    }
}
