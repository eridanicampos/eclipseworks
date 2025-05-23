﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Infrastructure.Data.Repositories.Common;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Services;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Repositories;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Application.Validators;
using FluentValidation;
using ProjectTest.Domain.Validators;

namespace ProjectTest.Infrastructure.Data.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot configuration)
        {
            AddDataBase(services, configuration);
            AddRepositoriesServices(services);
            AddStoreProcedures(services);
            AddValidators(services);
            return services;
        }

        private static void AddDataBase(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<ProjectTestContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProjectTest.Infrastructure.Data"));
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Scoped);
        }

        public static void AddRepositoriesServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient(typeof(IGenericAsyncRepository<>), typeof(GenericAsyncRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAcessoUsuarioRepository, AcessoUsuarioRepository>();
            services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            services.AddScoped<IComentarioRepository, ComentarioRepository>(); 
            services.AddScoped<IHistoricoAlteracaoRepository, HistoricoAlteracaoRepository>();
            services.AddScoped<ITarefaService, TarefaService>();
            services.AddScoped<IProjetoService, ProjetoService>();

            services.AddTransient<IEventPublisher, EventPublisher>();

        }

        public static void AddStoreProcedures(IServiceCollection services)
        {

        }
        public static void AddValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProjetoValidator>();
            services.AddValidatorsFromAssemblyContaining<TarefaValidator>();
            services.AddValidatorsFromAssemblyContaining<ComentarioValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateTarefaDtoValidator>();

        }
    }
}
