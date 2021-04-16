using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoupBot.Database.Models;
using MongoDB.Driver;

namespace CoupBot.Database.Repositories
{
    public abstract class BaseRepository<T> where T : Model
    {
        private readonly IMongoCollection<T> _collection;

        public BaseRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public Task InsertAsync(T entity)
        {
            return _collection.InsertOneAsync(entity, null, default);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return _collection.Find(filter).Limit(1).FirstOrDefaultAsync();
        }

        public async Task<List<T>> AllAsync(Expression<Func<T, bool>> filter = null)
        {
            var list = new List<T>();

            using var cursor = await _collection.Find(filter ?? Builders<T>.Filter.Empty).ToCursorAsync();
            
            while (await cursor.MoveNextAsync())
            {
                list.AddRange(cursor.Current);
            }

            return list;
        }

        public Task UpdateAsync(T entity)
        {
            return _collection.ReplaceOneAsync(y => y.Id == entity.Id, entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return (await _collection.Find(filter).Limit(1).CountDocumentsAsync()) != 0;
        }

        public Task ModifyAsync(T entity, Action<T> function)
        {
            function(entity);
            return UpdateAsync(entity);
        }

        public async Task ModifyAsync(Expression<Func<T, bool>> filter, Action<T> function)
        {
            var entity = await GetAsync(filter);

            function(entity);
            await UpdateAsync(entity);
        }

        public Task PushAsync(Expression<Func<T, bool>> filter, FieldDefinition<T> field, ulong value)
        {
            return _collection.UpdateOneAsync(filter, Builders<T>.Update.Push(field, value));
        }

        public Task PullAsync(Expression<Func<T, bool>> filter, FieldDefinition<T> field, ulong value)
        {
            return _collection.UpdateOneAsync(filter, Builders<T>.Update.Pull(field, value));
        }

        public Task<long> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return _collection.CountDocumentsAsync(filter ?? Builders<T>.Filter.Empty);
        }

        public Task DeleteAsync(T entity)
        {
            return _collection.DeleteOneAsync(y => y.Id == entity.Id);
        }

        public Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return _collection.DeleteManyAsync(filter);
        }
    }
}
