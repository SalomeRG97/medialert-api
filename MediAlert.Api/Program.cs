using Configuration;
using Model;

var builder = WebApplication.CreateBuilder(args);

// Primero configuramos servicios base
Database.ConfigureMySQLService(builder); // DbContext primero

// Identity debe ir después del DbContext
Configurations.ConfigureIdentity(builder);

// JWT necesita Identity y configuración lista
Configurations.ConfigureAuth(builder);

// AutoMapper, Repositorios, Servicios, Validaciones
Automapper.ConfigureService(builder);
BussinessLogic.Repository(builder);
BussinessLogic.Services(builder);
BussinessLogic.FluentValidation(builder);

// CORS y demás configuraciones generales
Configurations.ConfigurationServices(builder);
Configurations.AllowCORS(builder);

var app = builder.Build();

// Middlewares en orden
Configurations.ConfigureApp(app);
