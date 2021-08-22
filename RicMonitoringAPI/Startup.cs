using System;
using System.IO;
using System.Linq;
using System.Text;
using Audit.Core;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RicCommon.Enumeration;
using RicCommon.Services;
using RicCommunication.Interface;
using RicCommunication.PushNotification;
using RicCommunication.SmsGateway;
using RicEntityFramework;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Interfaces.PropertyMappings;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicEntityFramework.RoomRent.PropertyMappings;
using RicEntityFramework.RoomRent.PropertyMappings.Audits;
using RicEntityFramework.RoomRent.Repositories;
using RicEntityFramework.RoomRent.Repositories.Audits;
using RicEntityFramework.Services;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
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

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IRentTransactionRepository, RentTransactionRepository>();
            services.AddScoped<IRentTransactionDetailRepository, RentTransactionDetailRepository>();
            services.AddScoped<ILookupTypeItemRepository, LookupTypeItemRepository>();
            services.AddScoped<IRentArrearRepository, RentArrearRepository>();
            services.AddScoped<IMonthlyRentBatchRepository, MonthlyRentBatchRepository>();
            services.AddScoped<IRentTransactionHistoryRepository, RentTransactionHistoryRepository>();
            services.AddScoped<IRentTransactionPaymentRepository, RentTransactionPaymentRepository>();
            services.AddScoped<IMobileAppLogRepository, MobileAppLogRepository>();
            services.AddScoped<ISmsGatewayRepository, SmsGatewayRepository>();
            services.AddScoped<IRenterCommunicationRepository, RenterCommunicationRepository>();
            services.AddScoped<IAccountBillingItemRepository, AccountBillingItemRepository>();

            services.AddScoped<IAuditAccountRepository, AuditAccountRepository>();
            services.AddScoped<IAuditRenterRepository, AuditRenterRepository>();
            services.AddScoped<IAuditRoomRepository, AuditRoomRepository>();
            services.AddScoped<IAuditRentTransactionRepository, AuditRentTransactionRepository>();
            services.AddScoped<IAuditRentTransactionPaymentRepository, AuditRentTransactionPaymentRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;

                return new UrlHelper(actionContext);
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IRoomPropertyMappingService, RoomPropertyMappingService>();
            services.AddTransient<IRenterPropertyMappingService, RenterPropertyMappingService>();
            services.AddTransient<IRentTransactionPropertyMappingService, RentTransactionPropertyMappingService>();
            services.AddTransient<ILookupTypePropertyMappingService, LookupTypePropertyMappingService>();
            services.AddTransient<ILookupTypeItemPropertyMappingService, LookupTypeItemPropertyMappingService>();
            services.AddTransient<IRentTransactionHistoryPropertyMappingService, RentTransactionHistoryPropertyMappingService>();

            services.AddTransient<IAccountPropertyMappingService, AccountPropertyMappingService>();

            services.AddTransient<IAuditRenterPropertyMappingService, AuditRenterPropertyMappingService>();
            services.AddTransient<IAuditRoomPropertyMappingService, AuditRoomPropertyMappingService>();
            services.AddTransient<IAuditRentTransactionPropertyMappingService, AuditRentTransactionPropertyMappingService>();
            services.AddTransient<IAuditRentTransactionPaymentPropertyMappingService, AuditRentTransactionPaymentPropertyMappingService>();


            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddTransient<ISmsGatewayService, SmsGatewayService>();
            services.AddTransient<ICommunicationService, CommunicationService>();
            services.AddTransient<ISMSGateway, SMSGlobal>();

            // Get the service provider to access the http context
            var svrProvider = services.BuildServiceProvider();
            var settingRepository = svrProvider.GetService<ISettingRepository>();

            //DI with passing of constructor parameter
            services.AddTransient<IPushNotificationGateway>(p => 
                new OneSignalGateway(settingRepository.GetValue(SettingNameEnum.OneSignalAuthKey), 
                                     settingRepository.GetValue(SettingNameEnum.OneSignalAppId)));
            
            services.AddHealthChecks();

            // Add the HttpContextAccessor if needed.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .AuditTypeExplicitMapper(m => m
                        .Map<Account, AuditAccount>()
                        .Map<Room, AuditRoom>()
                        .Map<Renter, AuditRenter>()
                        .Map<RentTransaction, AuditRentTransaction>()
                        .Map<RentTransactionPayment, AuditRentTransactionPayment>()
                        .AuditEntityAction<IAudit>((evt, entry, auditEntity) =>
                        {
                            // Get the current HttpContext 
                            //var httpContext = svrProvider.GetService<IHttpContextAccessor>().HttpContext;
                            var userName = "TODO_USERNAME"; //httpContext.User?.Claims.FirstOrDefault(o => o.Type == "UserName")?.Value;

                            auditEntity.AuditDateTime = DateTime.UtcNow;
                            auditEntity.Username = string.IsNullOrEmpty(userName) ? "MANUAL_CALL" : userName;
                            auditEntity.AuditAction = entry.Action;
                        })
                    )
                );

            services.AddMvcCore()
                .AddAuthorization()
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }); //use for data shaping;

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .WithOrigins(Configuration["ClientUrl"]) 
                        .WithMethods("GET", "PUT", "POST", "DELETE");
                });
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = bool.Parse(Configuration["RequireHttpsMetadata"]);
                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidAudience = Configuration["JWT:ValidAudience"],
                                ValidIssuer = Configuration["JWT:ValidIssuer"],
                                IssuerSigningKey = new SymmetricSecurityKey(WebEncoders.Base64UrlDecode(Configuration["JWT:Secret"])),
                            };
                        }); 

          

            services.AddAuthorization(config =>
            {
                config.AddPolicy("SuperAndAdmin", policy => policy.RequireRole("Superuser", "Administrator"));
                config.AddPolicy("ProcessTenantsTransaction", policy => policy.RequireRole("Superuser", "Administrator", "Staff", "RunDailyBatch"));
                config.AddPolicy("Admin", policy => policy.RequireRole("Administrator"));
            });

            services.AddMvc(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = bool.Parse(Configuration["RequireHttpsMetadata"]); ;
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

            //remove the data protection log
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@".\logs\shared"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
            //,UserManager<IdentityUser> userManager
            )
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Audit.Core.Configuration.AuditDisabled = true;
            }
            else
            {
                //check https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                //settings
                cfg.CreateMap<Setting, SettingDto>()
                    .ForMember(dest => dest.DataType,
                                opt => opt.MapFrom(src => src.GetDataType()))
                    .ForMember(dest => dest.RealValue,
                        opt => opt.MapFrom(src => src.GetRealValue()));

                //rooms
                cfg.CreateMap<RoomForCreateDto, Room>();
                cfg.CreateMap<Room, RoomDto>();

                //renters
                cfg.CreateMap<RenterForCreateDto, Renter>();
                cfg.CreateMap<Renter, RenterDto>();

                //transactions
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
                                                opt => opt.MapFrom(src => src.GetPreviousBalance() + src.Renter.RentArrears.GetManualUnpaidAmountEntry()))
                    .ForMember(dest => dest.PaidOrUsedDepositDateString,
                        opt => opt.MapFrom(src => src.GetPaidOrUsedDepositDate()))
                    .ForMember(dest => dest.MonthlyRent,
                        opt => opt.MapFrom(src => src.GetMonthlyRent()))
                    .ForMember(dest => dest.CurrentBalance,
                        opt => opt.MapFrom(src => src.Balance))
                    .ForMember(dest => dest.IsDepositUsed, 
                        opt => opt.MapFrom(src => src.CheckIfUsedDeposit()))
                    .ForMember(dest => dest.BalanceDateToBePaidString, 
                        opt => opt.MapFrom(src => src.GetBalanceDateToBePaid()))
                    .ForMember(dest => dest.Payments,
                        opt => opt.MapFrom(src => src.GetPayments()));

                cfg.CreateMap<LookupType, LookupTypeDto>();
                cfg.CreateMap<LookupTypeItems, LookupTypeItemDto>();
                cfg.CreateMap<RentTransactionPayment, RentTransactionPaymentDto>()
                    .ForMember(dest => dest.DatePaidString, 
                                opt => opt.MapFrom(src => src.DatePaid.ToShortDateString()))
                    .ForMember(dest => dest.PaymentTransactionType,
                        opt => opt.MapFrom(src => src.GetTransactionPaymentType()));
            });

            //Enable CORS policy "AllowCors"
            app.UseCors("AllowCors");

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            //uncomment when deploy to live server
            //app.UseHttpsRedirection();

            // Matches request to an endpoint.
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // Execute the matched endpoint.
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
           
        }
    }
}
