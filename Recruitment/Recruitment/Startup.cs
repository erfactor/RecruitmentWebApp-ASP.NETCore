using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using Recruitment.Data;
using Recruitment.Utils;

namespace Recruitment
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
            services.AddDbContextPool<RecruitmentDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RecruitmentDb"));
            });

            services.AddScoped<IJobOfferData, SqlJobOfferData>();
            services.AddScoped<IApplicationData, SqlApplicationData>();
            services.AddScoped<IStaffMemberData, SqlStaffMemberData>();
            
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
                    {
                        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = "B2C_1_sign_up_in";
                    })
                .AddOpenIdConnect("B2C_1_sign_up", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_up",options))
                .AddOpenIdConnect("B2C_1_sign_in", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_in",options))
                .AddOpenIdConnect("B2C_1_sign_up_in", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_up_in",options))
                .AddOpenIdConnect("B2C_1_edit_profile", options => SetOptionsForOpenIdConnectPolicy("B2C_1_edit_profile",options))
                .AddCookie();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Job Offers Api",
                    Version = "v1"
                });
            });
            
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/v1/swagger.json","Recruitment Api V1"); });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void SetOptionsForOpenIdConnectPolicy(string policy, OpenIdConnectOptions options)
        {
            options.MetadataAddress = "https://minirecruitment.b2clogin.com/minirecruitment.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=" + policy;
            options.ClientId = "48451f4d-ae5f-4b55-a1ef-c8f9cca5a65c";
            options.ResponseType = OpenIdConnectResponseType.IdToken;
            options.CallbackPath = "/signin/" + policy;
            options.SignedOutCallbackPath = "/signout/" + policy;
            options.SignedOutRedirectUri = "/";
//            options.TokenValidationParameters.NameClaimType = "name";
        }
    }
}