using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public class Automapper
    {
        public static void ConfigureService(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
