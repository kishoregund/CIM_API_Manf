using Infrastructure;
using Infrastructure.Persistence;
using Application;
using Microsoft.Extensions.Configuration;
using Application.Models;
using Infrastructure.Validations;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateSpecialCharactersAttribute>();
                options.Filters.Add<EnforceGlobalStringLengthAttribute>();
            });
            builder.Services.AddApplicationServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddInfrastructureServices(builder.Configuration);
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    //.WithOrigins("http://www.yahoo.com")
                    //.WithMethods("POST", "GET")
                    //.WithHeaders("accept", "content-type")
                    .AllowAnyHeader());
            });

            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);


            //var appSettings = appSettingsSection.Get<AppSettingsModel>();

            var app = builder.Build();

            // Database Initializer
            await app.Services.AddDatabaseInitializerAsync();

            app.UseCors("CorsPolicy");
            // Configure the HTTP request pipeline.            
            app.UseHttpsRedirection();
            //app.UseAuthorization();
            
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
