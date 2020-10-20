using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Remove("MysqlConnection");
            //config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("MysqlConnection", "Data Source=localhost;port=3306;Initial Catalog=videogameforum; User Id=root;password=;SslMode=none;convert zero datetime=True"));
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("MysqlConnection", "Data Source=eu-cdbr-west-03.cleardb.net;port=3306;Initial Catalog=heroku_cebd2c31faac3c5; User Id=b1cb5866162958;password=e3753057;SslMode=none;convert zero datetime=True"));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
