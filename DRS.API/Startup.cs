using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.API.Controllers;
using DRS.Data;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Data.Services;
using DRS.Model.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DRS.API
{
    /* GenericList Services
     * DocumentRuleSettingService
     * DocumentService
     * DocumentCategoryService
     * DocumentFieldService
     * DocumentRuleService
     * */
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Data.Startup.Configuration();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            var connection = @"Server=ksju-intdb-ut.stb.intra,43305;Database=DRSDB;Trusted_Connection=True;Integrated Security=True;";
            services.AddDbContext<DRSDBContext>(options => options.UseSqlServer(connection));
            services.AddScoped<UnitOfWork>();

            services.AddScoped<IGenericService<ProcessSchemaStepSettingEntity>, ProcessSchemaStepSettingService>();
            services.AddScoped<IGenericListService<DocumentRuleSettingEntity>, DocumentRuleSettingService>();
            services.AddScoped<IGenericService<ProcessSchemaEntity>, ProcessSchemaService>();
            services.AddScoped<IGenericService<ArchiveEntity>,ArchiveService>();
            services.AddScoped<IGenericListService<DocumentEntity>,DocumentService>();
            services.AddScoped<IGenericListService<DocumentCategoryEntity>,DocumentCategoryService> ();
            services.AddScoped<IGenericListService<DocumentFieldEntity>, DocumentFieldService>();
            services.AddScoped<IGenericService<DocumentFieldSettingEntity>,DocumentFieldSettingsService>();
            services.AddScoped<IGenericListService<DocumentRuleEntity>,DocumentRuleService>();
            services.AddScoped<IGenericService<DocumentSourceEntity>,DocumentSourceService>();
            services.AddScoped<IGenericService<ProcessHandlerEntity>,ProcessHandlerService>();
            services.AddScoped<IGenericService<ProcessQueueEntity>,ProcessQueueService>();
            services.AddScoped<IGenericService<SystemSettingEntity>,SystemSettingService>();
            services.AddScoped<IGenericService<FieldEntity>, FieldService>();
            services.AddScoped<IGenericService<DocumentFieldParameterEntity>, DocumentFieldParameterService>();
            services.AddScoped<IGenericService<ProcessLogEntity>, ProcessLogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseCors(builder => builder.AllowAnyOrigin().WithMethods("GET", "POST", "HEAD","PUT","DELETE","OPTIONS").AllowAnyHeader());
            app.UseMvc();
        }
    }
}
