using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.API.Data;
using AutoMapper;
using WebApp.API.Extensions;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDatabase(this.Configuration)
                .AddIdentity()
                .AddJwtAuthentication(this.Configuration)
                .AddPolicyAuthorization()
                .AddApplicationServices()
                .AddCloudinarySettingsConfiguration(this.Configuration)
                .AddAutoMapper(typeof(Startup))
                .AddCors()
                .AddHttpContextAccessor()
                .AddApiControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, Seed seeder*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else 
            {
                // global exception handler throws an exception when response headers contain non-ascii chars
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        // context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if(error != null) {
                            // context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                { 
                    endpoints.MapControllers(); 
                });
                
            // seeder.SeedData();
        }
    }
}
