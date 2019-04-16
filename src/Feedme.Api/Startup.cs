using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Feedme.Api.CompositionRoot;

namespace Feedme.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCustom();
            services.AddSwagger();
            services.AddDbContextAndMigrate(Configuration);
            services.AddSerilog(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionMiddleware(env);
            app.UseHttps();
            app.UseMvc();
            app.UseSwaggerCustom();
        }

        public void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterModule(new AutofacModule(Configuration));
        }
    }
}
