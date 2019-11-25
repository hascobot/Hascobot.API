using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Core.UOW;
using Hasco.Bot.Infrastructure.Extensions;
using Hasco.Bot.Infrastructure.Mappers;
using Hasco.Bot.Infrastructure.Repositories;
using Hasco.Bot.Infrastructure.Services;
using Hasco.Bot.Infrastructure.Twitch;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;

namespace Hasco.Bot.Api
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
            services.AddMvc(option =>
                option.EnableEndpointRouting = false
            ).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSession();
            services.AddCors(options =>

            {

                options.AddPolicy("angular",

                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddAuthorization();


            services.AddScoped<IChatUserRepository, ChatUserRepository>();
            services.AddScoped<IChatUserService, ChatUserService>();
            services.AddScoped<IClientUserRepository, ClientUserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBadWordRepository, BadWordRepository>();
            services.AddSingleton(AutoMapperConfig.Initialize());
            services.AddSingleton<TwitchBot>();


            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               // x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("TestDb"));
            //services.AddDbContext<AppDbContext>(options =>
            //  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseMvc();
            app.UseCors("angular");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                pattern: "{controller}/{action}/{id?}",
                 defaults: new { controller = "Home", action = "Index" });
              //  endpoints.MapRazorPages();
                //endpoints.MapControllers();
            });

        }
    }
}
