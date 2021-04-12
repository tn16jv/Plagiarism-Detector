using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Floss.Api.Auth;
using Floss.Api.Authorization;
using Floss.Database.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Floss.Api.Filters;
using Floss.Api.Helpers;
using Floss.Api.FileToModel;
using Floss.Api.Controllers;

namespace Floss.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var authCfgSection = Configuration.GetSection("OAuthProviderConfig");
            var authCfg = authCfgSection.Get<OAuthProviderConfig>();

            services.Configure<OAuthProviderConfig>(authCfgSection);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.Audience = authCfgSection.GetValue<string>(nameof(OAuthProviderConfig.AppJwtAudience)) ?? "n/a";
                // DO NOT SPECIFY AUTORITY HERE - IT TRIGGERS AUTOMATIC REDIRECTION
                options.ClaimsIssuer = options.Authority;
                options.TokenValidationParameters = CreateTokenValidationParams(authCfg);
            });

            services.AddSingleton<StsOptions, StsOptions>();
            services.AddSingleton<OAuthUtil, OAuthUtil>();
            services.AddSingleton<PermissionHandlerBusinessAccount, PermissionHandlerBusinessAccount>();

			services.AddDbContext<FlossContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("floss")));
            Console.WriteLine("using connection " + Configuration.GetConnectionString("floss"));

            // Adding Role services to Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<FlossContext>()
               .AddDefaultTokenProviders();
   

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthenticationFilter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            

            // Add Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RoleProfessor", policy =>
                    policy.Requirements.Add(new RoleProfessorRequirement()));
                options.AddPolicy("RoleSystemAdmin", policy =>
                    policy.Requirements.Add(new RoleSystemAdminRequirement()));
                options.AddPolicy("RoleStudent", policy =>
                    policy.Requirements.Add(new RoleStudentRequirement()));
                options.AddPolicy("RoleGroupCoordinator", policy =>
                    policy.Requirements.Add(new RoleGroupCoordinatorRequirement()));
            });

            // Authorization handlers.
            services.AddScoped<IAuthorizationHandler,
                                    PermissionHandler>();

            
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Floss.Api", Version = "v1" });
                c.IncludeXmlComments("Floss.Api.xml", true);

                c.OperationFilter<Controllers.FileUploadController>(); //for custom swagger options 
            });


            ServiceProviderFactory.ServicesCollection = services;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName=="Local")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Floss.Api v1");
                //c.RoutePrefix = string.Empty;
            });
            //Use cors policy defined in Configure Services
            app.UseCors("CorsPolicy");
			app.UseMvc();
        }

        /// <summary>
        /// Configure tken validation parameters for applications-pecific JWT
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        TokenValidationParameters CreateTokenValidationParams(OAuthProviderConfig cfg)
        {
            var symmetricKey = Convert.FromBase64String(cfg.AppJwtSecret);

            var rz = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),

                ValidateIssuer = true,
                ValidIssuer = cfg.AppJwtAuthority,

                ValidateAudience = true,
                ValidAudience = cfg.AppJwtAudience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // allow up to 5 minutes clock skew between farm servers
            };
            return rz;
        }
    }
}
