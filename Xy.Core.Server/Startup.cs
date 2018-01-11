using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xy.Core.Server.CustomProvider;
using Xy.Core.Server.Services;


namespace Xy.Core.Server
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
            // Add identity types
            services.AddIdentity<MyApplicationUser, ApplicationRole>(
                    options =>
                    {
                        // 配置身份选项
                        // 密码配置
                        options.Password.RequireDigit = false; //是否需要数字(0-9).
                        options.Password.RequiredLength = 6; //设置密码长度最小为6
                        options.Password.RequireNonAlphanumeric = false; //是否包含非字母或数字字符。
                        options.Password.RequireUppercase = false; //是否需要大写字母(A-Z).
                        options.Password.RequireLowercase = false; //是否需要小写字母(a-z).

                        // 锁定设置
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); //账户锁定时长30分钟
                        options.Lockout.MaxFailedAccessAttempts = 10; //10次失败的尝试将账户锁定
                    })
                .AddDefaultTokenProviders();

            // Identity Services
            services.AddTransient<IUserStore<MyApplicationUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
            services.AddTransient<DapperUsersTable>();


            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<MyApplicationUser>();


            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
