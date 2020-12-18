using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.EntityFrameworkCore;
using SwaggerIntegration.API.SwaggerVersionMgmt;
using SwaggerIntegration.DAL.Contexts;

namespace SwaggerIntegration
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
            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();
            services.AddApiVersioning(ConfigureApiVersioning);
            services.AddSwaggerGen(ConfigureSwaggerDocumentation);
        }

        private void ConfigureApiVersioning(ApiVersioningOptions opts)
        {
            opts.ReportApiVersions = true;
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.DefaultApiVersion = new ApiVersion(1,0);
        }

        public void ConfigureSwaggerDocumentation(SwaggerGenOptions opts)
        {
            // version 1 of API
            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "ToDo API v1"
            });

            //version 2 of API
            opts.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "ToDo API v2"
            });

            opts.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            opts.OperationFilter<RemoveVersionFromParameter>();
            opts.DocInclusionPredicate((version, desc) =>
            {
                var versions = desc.CustomAttributes().OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);

                var res = versions.Any(v => $"v{v.MajorVersion.ToString()}" == version);
                return res;
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opts.IncludeXmlComments(xmlPath);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Integration v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Swagger Integration v2");
            });

            app.UseHttpsRedirection().UseRouting().UseAuthentication().UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
