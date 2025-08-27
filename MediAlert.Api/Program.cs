using Configuration;

var builder = WebApplication.CreateBuilder(args);

Automapper.ConfigureService(builder);
BussinessLogic.Repository(builder);
BussinessLogic.Services(builder);
BussinessLogic.FluentValidation(builder);
Configurations.ConfigurationServices(builder);
Configurations.ConfigureIdentity(builder);
Configurations.ConfigureAuth(builder);
Configurations.AllowCORS(builder);
Database.ConfigureMySQLService(builder);

var app = builder.Build();

Configurations.ConfigureApp(app);
