using backend.Infrastructure.Persistence.MongoDB;
using backend.Infrastructure.Persistence.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BurritosDbContext>(options =>
                options.UseMySql(sqlConnection,
                    Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(sqlConnection),
                    b => b.MigrationsAssembly(typeof(BurritosDbContext).Assembly.FullName)
                ));

            var mongoConnection = configuration.GetConnectionString("MongoConnection");

            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnection));

            services.AddScoped<IMongoDatabase>(sp => {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = MongoUrl.Create(mongoConnection).DatabaseName;
                return client.GetDatabase(databaseName);
            });

            services.AddScoped<BurritosMongoContext>();

            return services;
        }
    }
}
