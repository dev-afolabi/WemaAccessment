using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using WemaAssessment.API.Filters;
using WemaAssessment.API.Validators;
using WemaAssessment.Application.DTOs;
using WemaAssessment.Application.Services.Implementations;
using WemaAssessment.Application.Services.Interfaces;
using WemaAssessment.Application.Settings;
using WemaAssessment.Domain.Models;
using WemaAssessment.Persistence;
using WemaAssessment.Persistence.Repositories;

namespace WemaAssessment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddNewtonsoftJson(options =>  
                {  
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })  
                .AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WemaAssessment.API", Version = "v1" });
                c.ParameterFilter<StateParameterFilter>();
                c.ParameterFilter<LocalGovernmentParameterFilter>();
                
            }).AddSwaggerGenNewtonsoftSupport();

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<Customer, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IValidator<AddCustomerDto>, AddCustomerModelValidator>();
            services.AddTransient<IValidator<ConfirmPhoneNumberDto>, ConfirmPhoneNumberModelValidator>();

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<BankSettings>(Configuration.GetSection(BankSettings.ConfigSection));
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOtpService, OtpService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WemaAssessment.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Seed the database with some users and the state with lga
            DatabaseSeeder.EnsurePopulated(app).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
