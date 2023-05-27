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
            try
            {
                var filter = Builders<T>.Filter.Eq(IdConstantName, id);
                return await _collection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateAsync(T entity)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(IdConstantName, (entity as dynamic)._id);
                await _collection.ReplaceOneAsync(filter, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(T entity)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(IdConstantName, (entity as dynamic)._id);
                await _collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _collection.Find(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RemoveAsync(T entity)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq(IdConstantName, entity.GetType().GetProperty(IdConstantName).GetValue(entity));
                await _collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
