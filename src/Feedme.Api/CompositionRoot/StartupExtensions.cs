using Feedme.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Feedme.Api.Utils;

namespace Feedme.Api.CompositionRoot
{
    public static class StartupExtensions
    {
        public static void AddDbContextAndMigrate(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContextPool<AppDbContext>(x => x.UseSqlServer(connectionString));
            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();
        }

        public static void AddMvcCustom(this IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "Feedme API", Version = "v1" });
            });
        }

        public static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void UseExceptionMiddleware(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMiddleware<DevExceptionHandler>();
            }
            else
            {
                app.UseMiddleware<ProdExceptionHandler>();
            }
        }
        
        public static void UseHttps(this IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        public static void UseSwaggerCustom(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedme v1");
            });
        }
    }
}