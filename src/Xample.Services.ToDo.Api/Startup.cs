using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Xample.Services.Todo.Api;
using Xample.Services.ToDo.Business;
using Xample.Services.ToDo.Business.Interfaces;
using Xample.Services.ToDo.DataAccess;

namespace Xample.Services.ToDo.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(
              options =>
              {

                  options.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                  options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                  {
                      Type = "oauth2",
                      Flow = "implicit",
                      AuthorizationUrl = $"AuthorizationUrl",
                      Scopes = new Dictionary<string, string>
                      {
                      }
                  });
                  options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                  {
                        { "oauth2", new string[] { } }
                  });
              });

            // Inject an automapper implementation into the IOC container
            var mapper = new MapperConfiguration(config => config.AddProfile(typeof(AutomapperProfile))).CreateMapper();
            services.AddSingleton(mapper);

            // Inject a todos service implementation (TodoService) into the IOC container
            services.AddSingleton<ITodoService, TodoService>();

            // Inject a todos repository implementation (InMemoryRepository) into the IOC container
            services.AddSingleton<ITodoRepository, InMemoryRepository>();

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "Authority";
                options.Audience = "Audience";
            });

            // Add cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo Api V1");
                c.OAuthClientId("OAuthClientId");

                // options.OAuthAppName("Swagger UI");
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", "Audience" } });
            });

            // 2. Enable authentication middleware
            app.UseAuthentication();

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
