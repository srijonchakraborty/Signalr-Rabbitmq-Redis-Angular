using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public class MongoDBRepository<T> : IRepository<T> where T : class
    {
        public readonly static string IdConstantName = "Id";
        public readonly static string Id = "Id";
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabase _database;
        public MongoDBRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<T>(typeof(T).Name);
        }
        public async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(IdConstantName, ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(IdConstantName, (entity as dynamic)._id);
            await _collection.ReplaceOneAsync(filter, entity);
        }
        public async Task DeleteAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(IdConstantName, (entity as dynamic)._id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(IdConstantName, entity.GetType().GetProperty(IdConstantName).GetValue(entity));
            await _collection.DeleteOneAsync(filter);
        }

    }
}
