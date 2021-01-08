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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pdstest.services;
using pdstest.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using pdstest.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.OpenApi.Models;

namespace pdstest
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
            services.AddCors();
            services.AddControllers();
            services.AddSingleton<IWorker, Worker>();
            services.AddHostedService<MyCustomBackgroundService>();
            ////services.AddTransient<IConnection, MySqlOps>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "pds.com",
                        ValidAudience = "pds.com",
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PennaDeliveryServices"))

                    };
                }
                );
            services.Add(new ServiceDescriptor(typeof(IConnection),typeof(MySqlOps),ServiceLifetime.Scoped));
            services.AddSwaggerGen(s => s.SwaggerDoc("v1",new OpenApiInfo() {Title="PDS-API",Version="v1" }));
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           // else { app.UseHsts(); }
            app.UseHttpsRedirection();
         
            app.UseRouting();

            app.UseAuthorization();
            app.UseMyMiddleware();
            //app.UseMiddleware<JwtMiddleware>();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // custom jwt auth middleware

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(sw => sw.SwaggerEndpoint("/swagger/v1/swagger.json", "API for PDS"));
            //app.UseMvc();
        }
    }
}
