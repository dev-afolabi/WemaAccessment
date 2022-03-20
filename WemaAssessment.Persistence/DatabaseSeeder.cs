using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WemaAssessment.Domain.Models;

namespace WemaAssessment.Persistence
{
    public static class DatabaseSeeder
    {
        private static string path = Path.GetFullPath(@"../WemaAssessment.Persistence/Data/");
        private const string adminPassword = "Secret@123";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            //Get the Db context
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (context.Users.Any())
            {
                return;
            }
            else
            {
                //Get Usermanager and rolemanager from IoC container
                var userManager = app.ApplicationServices.CreateScope()
                                              .ServiceProvider.GetRequiredService<UserManager<Customer>>();

                //Seed states with local government areas
                var states = GetSampleData<State>(File.ReadAllText(path + "states.json"));
                await context.States.AddRangeAsync(states);

                //Seed Users
                var customers = GetSampleData<Customer>(File.ReadAllText(path + "customers.json"));

                foreach (var customer in customers)
                {
                    customer.UserName = customer.Email;
                    await userManager.CreateAsync(customer);
                    var token = await userManager.GenerateChangePhoneNumberTokenAsync(customer, customer.PhoneNumber);
                    await userManager.ChangePhoneNumberAsync(customer, customer.PhoneNumber, token);
                }

                await context.SaveChangesAsync();
            }
        }
        private static List<T> GetSampleData<T>(string location)
        {
            var output = JsonSerializer.Deserialize<List<T>>(location, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }
    }
}
