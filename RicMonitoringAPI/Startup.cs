using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RicMonitoringAPI.Api.Services.Interfaces.PropertyMappings;
using RicMonitoringAPI.Api.Services.PropertyMappings;
using RicMonitoringAPI.Common;
using RicMonitoringAPI.Common.Services;
using RicMonitoringAPI.Common.Validators;
using RicMonitoringAPI.RicXplorer.Services;
using RicMonitoringAPI.RicXplorer.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Validators;
using RicMonitoringAPI.RoomRent.Helpers.Extensions;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Services.PropertyMappings;
using RicMonitoringAPI.Services.Interfaces;
using RicMonitoringAPI.Services.RenterRent;
using RicMonitoringAPI.Services.RoomRent;

namespace RicMonitoringAPI
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
            

            //mapped database connection string
            services.AddDbContext<RoomRentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RicMonitoryDbConnString")));
            
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IRentTransactionRepository, RentTransactionRepository>();
            services.AddScoped<IRentTransactionDetailRepository, RentTransactionDetailRepository>();
            services.AddScoped<ILookupTypeItemRepository, LookupTypeItemRepository>();
            services.AddScoped<IRentArrearRepository, RentArrearRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;

                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IRoomPropertyMappingService, RoomPropertyMappingService>();
            services.AddTransient<IRenterPropertyMappingService, RenterPropertyMappingService>();
            services.AddTransient<IRentTransactionPropertyMappingService, RentTransactionPropertyMappingService>();
            services.AddTransient<ILookupTypePropertyMappingService, LookupTypePropertyMappingService>();
            services.AddTransient<ILookupTypeItemPropertyMappingService, LookupTypeItemPropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHealthChecks();

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        //.WithOrigins("http://localhost:4200")
                        .WithMethods("GET", "PUT", "POST", "DELETE")
                        .AllowAnyHeader();
                });
            });

            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.InputFormatters.Add(new XmlSerializerInputFormatter());
                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                setupAction.Filters.Add(typeof(ValidatorActionFilter));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<RoomForCreateDtoValidator>()) // this line automatically register all validators that inherit from AbstractValidator
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            }); //use for data shaping

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            RoomRentContext context
            //,UserManager<IdentityUser> userManager
            )
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg => {

                cfg.CreateMap<RoomForCreateDto, Room>();
                cfg.CreateMap<Room, RoomDto>();

                cfg.CreateMap<RenterForCreateDto, Renter>();
                cfg.CreateMap<Renter, RenterDto>();

                cfg.CreateMap<RentTransactionForCreateDto, RentTransaction>();
                cfg.CreateMap<RentTransaction, RentTransactionDto>();

                cfg.CreateMap<RentTransaction2, RentTransaction2Dto>()
                    .ForMember(dest => dest.DueDate,
                                opt => opt.MapFrom(src => src.GetDueDate()))
                    .ForMember(dest => dest.Period,
                                            opt => opt.MapFrom(src => src.GetPeriod()));

                cfg.CreateMap<LookupType, LookupTypeDto>();
                cfg.CreateMap<LookupTypeItems, LookupTypeItemDto>();

            });

            //Enable CORS policy "AllowCors"
            app.UseCors("AllowCors");

            app.UseHttpsRedirection();
            app.UseMvc();

            //seeding initial data here
            DbInitializer.Initialize(context);
        }
    }
}
