using Newtonsoft.Json;
using RabbitConsumerForNotification.Builder;
using RabbitConsumerForNotification.Constants;
using RabbitConsumerForNotification.Consumers;
using System.Configuration;
using System.Net.Http.Json;

namespace RabbitConsumerForNotification
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            AppSettingsBuilder.AppSettingsBuild();
            SpecificTaskConsumer.StartListenToRabbitMQ();
            Console.WriteLine($"Setting value: {JsonConvert.SerializeObject(CustomConstant.CurrentAppSettings)}");

            Console.WriteLine("Hello, World!!");
            while ( true )
            {
                Console.ReadKey();
            }
        }
    }
}