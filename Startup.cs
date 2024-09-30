using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WorkingHoursCounterSystemCore.DataWrapper;
using WorkingHoursCounterSystemCore.Interfaces;
using WorkingHoursCounterSystemCore.Services;

namespace WorkingHoursCounterSystemCore
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup()
        {
            configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<GoogleAuthLogic>();
            services.AddSingleton<AuthorizeLogic>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<DashboardLogic>();

            services.AddSingleton<DashboardDbContext>();
            services.AddSingleton<AuthorizeDbContext>();
            services.AddSingleton<ShiftDbContext>();
            services.AddSingleton<UserDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(k => { k.WithMethods("POST", "GET", "PATCH", "PUT"); k.AllowAnyOrigin(); k.AllowAnyHeader(); });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().AllowAnonymous();
                endpoints.MapSwagger();
                endpoints.MapControllers().AllowAnonymous();
            });
        }
    }
}
