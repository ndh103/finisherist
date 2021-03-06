using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonHub.Api.Common.DependencyInjections;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PersonHub.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationOptions(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAll",
                                builder =>
                                {
                                    builder.AllowAnyHeader();
                                    builder.AllowAnyMethod();
                                    builder.AllowAnyOrigin();
                                });
            });

            services.AddControllersWithViews();

            AddAuthentication(services);

            services.AddApplicationDbContexts(Configuration);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonHub.Api", Version = "v1" });

                //TODO: add authentication for swagger
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonHub.Api V1");
                    // options.OAuthClientId("swagger-client");
                    // options.OAuthClientSecret("swagger-secret".Sha256());
                    // options.OAuthAppName("Demo API - Swagger");
                    // options.OAuthUsePkce();
                });
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        }
    }
}
