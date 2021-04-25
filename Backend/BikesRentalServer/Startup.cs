using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Filters;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

namespace BikesRentalServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(ErrorMappingFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

            services.AddTransient<IStationsService, StationsService>();
            services.AddTransient<IBikesService, BikesService>();
            services.AddTransient<IUsersService, UsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BikesRentalServer API");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
