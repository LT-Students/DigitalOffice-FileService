using FluentValidation;
using HealthChecks.UI.Client;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Broker.Consumers;
using LT.DigitalOffice.FileService.Business;
using LT.DigitalOffice.FileService.Business.Helpers;
using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Configuration;
using LT.DigitalOffice.FileService.Data;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Mappers.ModelMappers;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.FileService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddKernelExtensions();

            services.AddHealthChecks();

            string connStr = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = Configuration.GetConnectionString("SQLConnectionString");
            }

            services.AddDbContext<FileServiceDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });
            services.AddControllers();

            services.AddHealthChecks()
                .AddSqlServer(connStr);

            ConfigureCommands(services);
            ConfigureMappers(services);
            ConfigureRepositories(services);
            ConfigureValidators(services);
            ConfigureMassTransit(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqOptions.RabbitMqSectionName)
                .Get<RabbitMqConfig>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetFileConsumer>();
                x.AddConsumer<AddImageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{rabbitMqConfig.Username}_{rabbitMqConfig.Password}");
                        host.Password(rabbitMqConfig.Password);
                    });

                    cfg.ReceiveEndpoint(rabbitMqConfig.GetFileEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<GetFileConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(rabbitMqConfig.AddImageEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<AddImageConsumer>(context);
                    });
                });

                x.AddRequestClient<ICheckTokenRequest>(
                  new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.ValidateTokenEndpoint}"));

                x.ConfigureKernelMassTransit(rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IAddNewFileCommand, AddNewFileCommand>();
            services.AddTransient<IGetFileByIdCommand, GetFileByIdCommand>();
            services.AddTransient<IDisableFileByIdCommand, DisableFileByIdCommand>();

            services.AddTransient<IAddNewImageCommand, AddNewImageCommand>();
            services.AddTransient<IImageResizeAlgorithm, ImageToSquareAlgorithm>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, FileServiceDbContext>();

            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IFileMapper, FileMapper>();
            services.AddTransient<IImageRequestMapper, ImageRequestMapper>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<File>, FileValidator>();
            services.AddTransient<IValidator<ImageRequest>, ImageRequestValidator>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            UpdateDatabase(app);

            app.AddExceptionsHandler(loggerFactory);

            app.UseMiddleware<TokenMiddleware>();

#if RELEASE
            app.UseHttpsRedirection();
#endif

            app.UseRouting();

            string corsUrl = Configuration.GetSection("Settings")["CorsUrl"];

            app.UseCors(builder =>
                builder
                    .WithOrigins(corsUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            var rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqOptions.RabbitMqSectionName)
                .Get<RabbitMqConfig>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks($"/{rabbitMqConfig.Password}/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<FileServiceDbContext>();

            context.Database.Migrate();
        }
    }
}