using MongoDB.Bson;

namespace Payment.DAL.Entities
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; } 
    }
}
