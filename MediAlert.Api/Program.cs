using Configuration;
using Model;

var builder = WebApplication.CreateBuilder(args);

// Primero configuramos servicios base
Database.ConfigureMySQLService(builder); // DbContext primero

// Identity debe ir despu�s del DbContext
Configurations.ConfigureIdentity(builder);

// JWT necesita Identity y configuraci�n lista
Configurations.ConfigureAuth(builder);

// AutoMapper, Repositorios, Servicios, Validaciones
Automapper.ConfigureService(builder);
BussinessLogic.Repository(builder);
BussinessLogic.Services(builder);
BussinessLogic.FluentValidation(builder);

// CORS y dem�s configuraciones generales
Configurations.ConfigurationServices(builder);
Configurations.AllowCORS(builder);

var app = builder.Build();

// Middlewares en orden
Configurations.ConfigureApp(app);
