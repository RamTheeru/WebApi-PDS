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
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace pdstest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private static bool isCloud = false;
        public IConfiguration Configuration { get; }
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            if (_httpContext != null)
            {
                isCloud = _httpContext.GetCloudEnvironment();
            }
            services.AddCors();
            services.AddControllers();
            services.AddRazorPages();
            services.AddSingleton<IWorker, Worker>();
            services.AddHostedService<MyCustomBackgroundService>();
            services.AddHostedService<MySqlBackupService>();
            services.AddWkhtmltopdf("pagetopdf");
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
            
           
            /// services.Add(new ServiceDescriptor(typeof(IConnection),typeof(MySqlOps),ServiceLifetime.Scoped));
            services.AddScoped<IConnection>(s => new MySqlOps(isCloud));
         
            services.AddSwaggerGen(s => s.SwaggerDoc("v1",new OpenApiInfo() {Title="PDS-API",Version="v1" }));
            services.AddDirectoryBrowser();
            
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
            ///&& !System.IO.Path.HasExtension(context.Request.Path.Value)
            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                isCloud = context.GetCloudEnvironment();
                MySQLDBOperations.isCloud = isCloud;
                await next();
                //if (context.Request.Host.Host.StartsWith("local") && context.Response.StatusCode == 404)
                //{
                //    context.Request.Path = context.Request.Host.Host + "/ClientApp";
                //    await next();
                //}
                //else if (context.Request.Host.Host.Contains("kleenandshine") && context.Response.StatusCode == 404)
                //{
                //    context.Request.Path = context.Request.Host.Host + "/ClientApp";
                //    await next();
                //}
                //if (context.Response.StatusCode == 404)
                //{
                //    context.Request.Path = "/ClientApp";
                //    await next();
                //}
            });
            //DefaultFilesOptions options = new DefaultFilesOptions();
            //options.DefaultFileNames.Clear();
            //options.DefaultFileNames.Add("/ClientApp/index.html");
            //app.UseDefaultFiles(options);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"PDSImages")),
                RequestPath = new PathString("/Images"),
                EnableDirectoryBrowsing = true
            });
    //        app.UseFileServer(new FileServerOptions()
    //        {
    //            FileProvider = new PhysicalFileProvider(
    //Path.Combine(Directory.GetCurrentDirectory(), @"ClientApp")),
    //            RequestPath = new PathString("/ClientApp"),
    //            EnableDirectoryBrowsing = true
    //        });
            //Enable directory browsing
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //                Path.Combine(Directory.GetCurrentDirectory(), @"PDSImages")),
            //    RequestPath = new PathString("/Images")
            //});

            app.UseRouting();

            app.UseAuthorization();
            app.UseMyMiddleware();
            //app.UseDefaultFiles(new DefaultFilesOptions
            //{
            //    DefaultFileNames = new
            //    List<string> { "home.html" }
            //});
            //app.UseMiddleware<JwtMiddleware>();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
               /// endpoints.MapRazorPages();
                //endpoints.Map
                 endpoints.MapControllers();
               // endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{id?}",defaults:new { controller="WeatherForecast",action="Index"});

            });
            //app.UseMvc(routes => {
            //    routes.MapRoute(name: "default",template:"",defaults:new { Controller });
            //});
            // custom jwt auth middleware

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(sw => sw.SwaggerEndpoint("./v1/swagger.json", "API for PDS"));
            //app.UseMvc();
           
   
        }
    }
}
