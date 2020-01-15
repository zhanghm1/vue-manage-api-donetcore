using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using VueManage.Api.IdentityServer4;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;
using System.Reflection;
using System.IO;
using VueManage.Domain;
using VueManage.Application;
using VueManage.Api.Filter;
using VueManage.Infrastructure.Common;
using VueManage.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace VueManage.Api
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
            
            services.AddControllers();
            services.AddDomain();
            services.AddInfrastructureEFCore(Configuration);
            services.AddInfrastructureCommon();
            
            services.AddApplication();

            services.AddMemoryCache();


            #region API�ĵ�½����
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResourceResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                //.AddAspNetIdentity<ApplicationUser>()
                ;



            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = false;
                options.Audience = "manage_api";
            });
            #endregion

            // ����api���ص�json��ʽ
            services.AddMvc(option =>
            {
                option.Filters.Add<AuthorizationFilter>();
                option.Filters.Add<ExceptionFilter>();
                
            })

            .AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    //Сд���»���
                    // NamingStrategy = new SnakeCaseNamingStrategy()

                    //����ĸСд
                    // NamingStrategy = new CamelCaseNamingStrategy()

                    //ͬ������
                    NamingStrategy = new DefaultNamingStrategy(),
                };

            });

            //vue��Ŀ����APIʱ���ܻ����ض����������ͷ��ʶΪajax����   X-Requested-With: XMLHttpRequest
            //���ÿ�����
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //�����κ���Դ����������
                    //.WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("any");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UserLanguageMiddleware();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
