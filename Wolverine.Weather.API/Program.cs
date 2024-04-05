using Wolverine.Weather.API.Profiles;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Services;
using Wolverine.Weather.Infrastructure.Profiles;
using Wolverine.Weather.Infrastructure.Repositories;
using Wolverine.Weather.Infrastructure.Settings;

namespace Wolverine.Weather.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Nolan: Added these servies
            builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
            builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
            builder.Services.Configure<DatabaseConfigurationSection>(builder.Configuration.GetSection("DatabaseConfiguration"));
            
            List<Type> scanTypes = new List<Type> { typeof(InfrastructureProfile), typeof(ViewModelProfiles) };
            builder.Services.AddAutoMapper(options =>
            {
                options.AllowNullCollections = true;
            }, scanTypes);
            //Nolan: end services added

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}