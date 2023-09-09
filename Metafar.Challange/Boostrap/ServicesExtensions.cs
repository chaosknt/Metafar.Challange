using Metafar.Challange.Data;
using Metafar.Challange.Data.Models;
using Metafar.Challange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Metafar.Challange.Common;

namespace Metafar.Challange.Boostrap
{
    public static class ServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MetafarDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("MetafarDb")));

            services.AddIdentity<MetafarAccDbEntity, RoleDbEntity>()
               .AddEntityFrameworkStores<MetafarDbContext>();

            //services.AddJWTTokenServices(configuration);
            //services.AddSwaggerGenService();

            //services.AddDataServices();
            //services.AddConfigurationService();

            return services;
        }

        public static void AddJWTTokenServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            var bindJwtSettings = new JwtSettings();
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            Services.AddSingleton(bindJwtSettings);
            Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
            });
        }

        public static void AddSwaggerGenService(this IServiceCollection Services)
        {
            Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services)
            => services
            .AddDataManagerServices()
            .AddDataStoreServices();

        internal static IServiceCollection AddDataManagerServices(this IServiceCollection services)
            => services;
        //.AddScoped<IAccountManager, MapaUserManager>()
        //.AddScoped<IUserManager, UserManager>()
        //.AddScoped<ICompanyManager, CompanyManager>()
        //.AddScoped<IRoleManager, RoleManager>()
        //.AddScoped<IZoneManager, ZoneManager>()
        //.AddScoped<ISellerManager, SellerManager>()
        //.AddScoped<ISellerNumbersManager, SellerNumbersManager>()
        //.AddScoped<INumbersManager, NumbersManager>()
        //.AddScoped<ICustomersManager, CustomersManager>()
        //.AddScoped<ICustomerClaimsManager, CustomerClaimsManager>()
        //.AddScoped<ICustomerDebtManager, CustomerDebtManager>()
        //.AddScoped<ICustomerOrderManager, CustomerOrderManager>()
        //.AddScoped<IAttendanceRecordsManager, AttendanceRecordsManager>();

        internal static IServiceCollection AddDataStoreServices(this IServiceCollection services)
            => services;
        //.AddScoped<IMapaAccountUserStore, MapaAccountUserStore>()
        //.AddScoped<IUserStore, UserStore>()
        //.AddScoped<ICompanyStore, CompanyStore>()
        //.AddScoped<IRoleStore, RoleStore>()
        //.AddScoped<IZoneStore, ZoneStore>()
        //.AddScoped<ISellerStore, SellerStore>()
        //.AddScoped<ISellerNumbersStore, SellerNumbersStore>()
        //.AddScoped<INumbersStore, NumbersStore>()
        //.AddScoped<ICustomerStore, CustomerStore>()
        //.AddScoped<ICustomerClaimsStore, CustomerClaimsStore>()
        //.AddScoped<ICustomerDebtStore, CustomerDebtStore>()
        //.AddScoped<ICustomerOrderStore, CustomerOrderStore>()
        //.AddScoped<IAttendanceRecordsStore, AttendanceRecordsStore>()
        //.AddScoped<ICheckOutRulesStore, CheckOutRulesStore>();

        public static IServiceCollection AddConfigurationService(this IServiceCollection services)
           => services
               .AddSingleton<AppSettingsConfigurationService>();
    }
}
