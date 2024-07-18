using System;
using System.IO;
using Audit.Core;
using FluentValidation.AspNetCore;
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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RicCommon.Enumeration;
using RicCommon.Services;
using RicCommunication.Interface;
using RicCommunication.PushNotification;
using RicCommunication.SmsGateway;
using RicEntityFramework;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
using RicMonitoringAPI.Common.Validators;
using RicMonitoringAPI.RoomRent.Validators;
using RicMonitoringAPI.Services;
using RicMonitoringAPI.Services.Interfaces;
using IdentityServer4.AccessTokenValidation;
using RicMonitoringAPI.MappingProfiles;
using System.Reflection;

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

            // Add AutoMapper configuration
            services.AddAutoMapper(typeof(RentProfile));
            services.AddAutoMapper(typeof(RicXplorerProfile));
            services.AddAutoMapper(typeof(CostMonitoringProfile));
            services.AddAutoMapper(typeof(ToolInventoryProfile));
            services.AddAutoMapper(typeof(BuklodProfile));

            // Register all services with transient lifetime
            // Get the assembly containing the services (e.g., MyServicesAssembly.dll)
            var ricEntityFrameworkServiceAssembly = Assembly.Load("RicEntityFramework");

            // Register all services from the specified assembly with transient or scope lifetime
            services.AddAllServicesFromAssemblies(ricEntityFrameworkServiceAssembly);

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IOneSignalService, OneSignalService>();
            services.AddTransient<ISMSGateway, SMSGlobal>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;

                return new UrlHelper(actionContext);
            });
            
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
                var clientUrls = Configuration["ClientUrl"].Split(',');
                options.AddPolicy("AllowCors", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .WithOrigins(clientUrls) 
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


            //Enable CORS policy "AllowCors"
            app.UseCors("AllowCors");

            //comment this line when deployed to aws
            //start
            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});
            //end 

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
