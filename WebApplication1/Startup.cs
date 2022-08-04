using JwtAuthentication;
using JwtAuthentication.Models;
using JwtAuthentication.Models.BaseModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // Reading Connection string from appsettings.json
            services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TRMConnection")));



            /*
             *=========================================== 
             *         JWT Auth Regiteration 
             *==========================================
             */


            // Step 2 JWTAuthentication
            var _jwtSetting = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSetting>(_jwtSetting);

            // For refreshtoken
            var dbContext = services.BuildServiceProvider().GetService<CustomerDbContext>();
            services.AddSingleton<RefreshTokenGenerator>(provider => new RefreshToken(dbContext));
            // end for refrestoken

            // Step 4 JWTAuthentication
            var authkey = Configuration.GetValue<string>("JWTSettings:securitykey");

            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // new package to install for JwtBearerDefaults -> using Microsoft.AspNetCore.Authentication.JwtBearer;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item => {

                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    //ValidIssuer = false,
                    //ValidAudience = false,
                };
            });


            /*
             *=========================================== 
             *         JWT Auth END
             *==========================================
             */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication(); // before 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
