using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization;
using BikesRentalServer.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BikesRentalServer.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(ErrorMappingFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            services.AddCors();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer Auth", new OpenApiSecurityScheme
                {
                    Name = "Bearer token",
                    Scheme = "bearer",
                    Description = "Authorization bearer token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                });
                options.OperationFilter<SwaggerAuthorizationOperationFilter>();
            });

            services.AddScoped<UserContext>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(_configuration.GetConnectionString("DefaultDatabase"));
            });

            services.AddScoped<AuthorizationFilter>();
            services.AddScoped<ErrorMappingFilter>();

            services.AddTransient<IBikesRepository, BikesRepository>();
            services.AddTransient<IStationsRepository, StationsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IReservationsRepository, ReservationsRepository>();
            services.AddTransient<IMalfunctionsRepository, MalfunctionsRepository>();

            services.AddTransient<IStationsService, StationsService>();
            services.AddTransient<IBikesService, BikesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IMalfunctionsService, MalfunctionsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = _configuration["PathBase"];
            app.UsePathBase(pathBase);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "BikesRentalServer API");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
