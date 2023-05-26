using Common.Model;
using MongoDB.Driver;
using Repository.Interface;
using Repository.Repository;
using SignalRPractice.ExtentionMethods;
using SignalRPractice.Hubs;
using SignalRPractice.RedisManager;
using System.Linq.Expressions;

namespace SignalRPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

            #region Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .WithOrigins(appSettings.CorsSetting[0],
                                 appSettings.CorsSetting[1])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            
            #region Register DB
            builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(appSettings.MongoConnection.ConnectionString));
            //Note: It Register's the IMongoDatabase for MongoDBRepository class
            builder.Services.AddScoped(s => s.GetService<IMongoClient>().GetDatabase(appSettings.MongoConnection.ConnectionString));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
            #endregion

            #region Redis Cache

            builder.Services.AddStackExchangeRedisCache(
            options =>
            {
                options.Configuration = appSettings.RedisConnection.ConnectionString;
                //options.InstanceName = appSettings.RedisConnection.InstanceName;
            });
            builder.Services.AddSingleton<IRedisService, RedisService>();
            #endregion

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.SetupSignalRHubs();
            app.Run();
        }      
    }
}