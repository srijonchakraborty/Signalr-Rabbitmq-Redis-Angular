using Common.Model;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitConsumerForNotification.Builder;
using RabbitConsumerForNotification.Constants;
using RabbitConsumerForNotification.Consumers;
using RabbitConsumerForNotification.DBService;
using Repository.Interface;
using Repository.Repository;
using System.Configuration;
using System.Net.Http.Json;

namespace RabbitConsumerForNotification
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            AppSettingsBuilder.AppSettingsBuild();
          
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IMongoClient>(s => new MongoClient(CustomConstant.CurrentAppSettings.MongoConnection.ConnectionString))
                .AddSingleton(s => s.GetService<IMongoClient>().GetDatabase(CustomConstant.CurrentAppSettings.MongoConnection.InstanceName))
                .AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>))
                .AddSingleton<ISpecificTaskService, SpecificTaskService>()
                .BuildServiceProvider();

            //SpecificTaskConsumer class use DI so we need CreateInstance like this process 
            var specificTaskConsumer = ActivatorUtilities.CreateInstance<SpecificTaskConsumer>(serviceProvider);
            specificTaskConsumer.StartListenToRabbitMQ();

            Console.WriteLine($"Setting value: {JsonConvert.SerializeObject(CustomConstant.CurrentAppSettings)}");

            Console.WriteLine("Hello, World!!");
            while ( true )
            {
                Console.ReadKey();
            }
        }
    }
}