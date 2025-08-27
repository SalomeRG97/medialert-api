using FluentValidation.AspNetCore;
using Interface.Repository.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Model.Data;
using Model.Identity;
using Repository.Base;
using Services.ErrorResponse;
using System.Text;

namespace Configuration
{
    public class Configurations
    {
        public static void ConfigurationServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddFluentValidationAutoValidation();
            //builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            // Registro de FluentValidation y validadores
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .Select(e => new
                            {
                                Campo = e.Key,
                                Mensajes = e.Value?.Errors.Select(x => x.ErrorMessage)
                            });

                        return new BadRequestObjectResult(new
                        {
                            Mensaje = "Error de validación",
                            Errores = errors
                        });
                    };
                });
        }

        public static void ConfigureIdentity(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MedsAppContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            });

        }

        public static void ConfigureAuth(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            var key = Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            builder.Services.AddAuthorization();
        }

        public static void AllowCORS(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirOrigenLocal",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                    });
            });
        }

        public static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedRoles.SeedRol(services);
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("PermitirOrigenLocal");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

    }
}
