using Common.Model;
using MongoDB.Driver;
using RabbitConsumerForNotification.Constants;
using Repository.Interface;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerForNotification.DBService
{
    internal class SpecificTaskService: ISpecificTaskService
    {
        private readonly IRepository<SpecificTaskLog> mongoDBRepository;
        private readonly MongoClient MongoClient;
        private readonly IMongoDatabase MongoDatabase;
        public SpecificTaskService(IRepository<SpecificTaskLog> mongoDBRepository)
        {
            this.mongoDBRepository = mongoDBRepository;
        }
        public async Task SaveSpecificTaskAsync(SpecificTaskLog specificTaskLog)
        {
           await mongoDBRepository.AddAsync(specificTaskLog);
        }
        public async Task<SpecificTaskLog> GetSpecificTaskBySubIdAsync(string subscriptionId)
        {
            Expression<Func<SpecificTaskLog, bool>> predicate = p => p.SubscriptionId== subscriptionId;
            var specificTaskLog = await mongoDBRepository.FindAsync(predicate);
            return specificTaskLog?.FirstOrDefault();
        }
        public async Task<SpecificTaskLog> GetSpecificTaskByIdAsync(string id)
        {
           var specificTaskLog=await mongoDBRepository.GetByIdAsync(id);
            return specificTaskLog;
        }
    }
}
