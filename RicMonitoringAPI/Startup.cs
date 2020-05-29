using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using RicEntityFramework;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Interfaces.PropertyMappings;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicEntityFramework.RoomRent.PropertyMappings;
using RicEntityFramework.RoomRent.Repositories;
using RicEntityFramework.Services;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Extensions;
using RicMonitoringAPI.Common.Validators;
using RicMonitoringAPI.RicXplorer.Services;
using RicMonitoringAPI.RicXplorer.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Helpers.Extensions;
using RicMonitoringAPI.RoomRent.Validators;

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
            services.AddDbContext<RicDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RicMonitoringApiDbConnString")));

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IRentTransactionRepository, RentTransactionRepository>();
            services.AddScoped<IRentTransactionDetailRepository, RentTransactionDetailRepository>();
            services.AddScoped<ILookupTypeItemRepository, LookupTypeItemRepository>();
            services.AddScoped<IRentArrearRepository, RentArrearRepository>();
            services.AddScoped<IMonthlyRentBatchRepository, MonthlyRentBatchRepository>();
            services.AddScoped<IRentTransactionHistoryRepository, RentTransactionHistoryRepository>();

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
            services.AddTransient<IRentTransactionHistoryPropertyMappingService, RentTransactionHistoryPropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddHealthChecks();

            services.AddMvcCore()
                .AddAuthorization()
                .AddNewtonsoftJson();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["authority"]; //auth server

                    options.RequireHttpsMetadata = true;

                    // name of the API resource //resourceApi
                    options.ApiName = "RicMonitoringAPI";
                });


            services.AddAuthorization(config =>
            {
                config.AddPolicy("Superuser", policy => policy.RequireRole("Superuser", "Administrator"));
                config.AddPolicy("ProcessTenantsTransaction", policy => policy.RequireRole("Superuser", "Administrator", "Staff"));
                config.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
            });

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder
                        //.AllowAnyOrigin()
                        .WithOrigins(Configuration["clientUrl"]) //client url
                        .WithMethods("GET", "PUT", "POST", "DELETE")
                        .AllowAnyHeader();
                });
            });

            services.AddMvc(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true;
                    setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions()));
                    setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    setupAction.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<RoomForCreateDtoValidator>()) // this line automatically register all validators that inherit from AbstractValidator
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }); //use for data shaping



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            RicDbContext context
            //,UserManager<IdentityUser> userManager
            )
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //check https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
                //app.UseExceptionHandler("/Error"); 
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {

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

                //histories
                cfg.CreateMap<RentTransaction, RentTransactionHistoryDto>()
                    .ForMember(dest => dest.PreviousBalance,
                                                opt => opt.MapFrom(src => src.GetPreviousBalance()))
                    .ForMember(dest => dest.MonthlyRent,
                        opt => opt.MapFrom(src => src.GetMonthlyRent()))
                    .ForMember(dest => dest.CurrentBalance,
                        opt => opt.MapFrom(src => src.Balance))
                    ;

                cfg.CreateMap<LookupType, LookupTypeDto>();
                cfg.CreateMap<LookupTypeItems, LookupTypeItemDto>();

            });

            //Enable CORS policy "AllowCors"
            app.UseCors("AllowCors");

            app.UseHttpsRedirection();
            // Matches request to an endpoint.
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // Execute the matched endpoint.
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
           
        }
    }
}
