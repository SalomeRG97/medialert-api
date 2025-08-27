using Interface.Repository.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repository.Base;

namespace Configuration
{
    public class BussinessLogic
    {
        public static void Repository(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void Services(WebApplicationBuilder builder)
        {
            //builder.Services.AddScoped<ITaskService, TaskService>();
        }

        public static void FluentValidation(WebApplicationBuilder builder)
        {
            //builder.Services.AddTransient<IValidator<TaskDTO>, TaskDTOValidator>();
        }
    }
}
