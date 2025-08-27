using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Data;

namespace Configuration
{
    public class Database
    {
        public static void ConfigureMySQLService(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<MedsAppContext>(
                options =>
                {
                    options.UseMySql(
                        connectionString,
                        new MySqlServerVersion(new Version(8, 0, 0)), // Especifica tu versión exacta
                        mySqlOptions =>
                        {
                            mySqlOptions.DisableBackslashEscaping();
                        }
                    );
                });
        }
    }
}
