using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RicAuthServer.Data;
using RicAuthServer.Services;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using RicAuthServer.RicAuthControllers;

namespace RicAuthServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RicAuthServerConnection")));

            //added IProfileService here
            services.AddTransient<IProfileService, ProfileService>();

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //when reset password the token should only valid for 2 hours.
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication("Bearer", options =>
            //    {
            //        //// name of the API resource //resourceApi
            //        options.ApiName = "RicMonitoringAPI";

            //        options.Authority = Configuration["authority"];
            //    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("AdminAndSuperuser", policy => policy.RequireRole("Administrator", "Superuser"));

                config.AddPolicy("Users", policy => policy.RequireRole("Staff", "Administrator", "Superuser"));

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

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizeFolder("/Account");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //AddIdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("RicAuthServerConnection"),
                            db => db.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("RicAuthServerConnection"),
                            db => db.MigrationsAssembly(migrationsAssembly));
                })
                .AddProfileService<ProfileService>();


            services.AddMvc(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true;
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
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors("AllowCors");
            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            //app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
