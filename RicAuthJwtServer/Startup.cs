using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RicAuthJwtServer.Application;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data;
using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Data.Persistence.Repositories;
using RicMonitoringAPI.Common.Validators;

namespace RicAuthJwtServer
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RicAuthServerConnection")));

            //Repositories
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAspNetUserLoginTokenRepository, AspNetUserLoginTokenRepository>();
            services.AddScoped<IRegisteredDeviceRepository, RegisteredDeviceRepository>();
            
            //Service
            services.AddTransient<IAspNetUserLoginTokenService, AspNetUserLoginTokenService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IRegisteredDeviceService, RegisteredDeviceService>();

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //when reset password the token should only valid for 1 hours.
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));

            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .WithMethods("GET", "PUT", "POST", "DELETE")
                        .AllowAnyHeader();
                });
            });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = bool.Parse(Configuration["RequireHttpsMetadata"]);
                options.TokenValidationParameters = new TokenValidationParameters() { 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("SuperAndAdmin", policy => policy.RequireRole("Superuser", "Administrator"));
                config.AddPolicy("ProcessTenantsTransaction", policy => policy.RequireRole("Superuser", "Administrator", "Staff"));
                config.AddPolicy("Admin", policy => policy.RequireRole("Administrator"));
            });

            services.AddMvc(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = bool.Parse(Configuration["RequireHttpsMetadata"]); ;
                    setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions()));
                    setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    setupAction.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }); //use for data shaping
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
