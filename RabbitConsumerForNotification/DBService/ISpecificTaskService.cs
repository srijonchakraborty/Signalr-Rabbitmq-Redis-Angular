using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerForNotification.DBService
{
    internal interface ISpecificTaskService
    {
        Task SaveSpecificTaskAsync(SpecificTaskLog specificTaskLog);
        Task<SpecificTaskLog> GetSpecificTaskBySubIdAsync(string subscriptionId);
        Task<SpecificTaskLog> GetSpecificTaskByIdAsync(string id);
    }
}
