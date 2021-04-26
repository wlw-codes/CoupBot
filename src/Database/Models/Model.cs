using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoupBot.Database.Models
{
    public abstract class Model
    {
        [BsonId] public ObjectId Id { get; set; }
    }
}