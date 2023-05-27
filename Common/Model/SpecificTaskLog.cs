using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Common.Model
{
    [Serializable]
    public class SpecificTaskLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }

        [BsonElement("Description")]
        [BsonRepresentation(BsonType.String)]
        public string? Description { get; set; }

        [BsonElement("SubscriptionId")]
        [BsonRepresentation(BsonType.String)]
        public string? SubscriptionId { get; set; }

        [BsonElement("Message")]
        [BsonRepresentation(BsonType.String)]
        public string? Message { get; set; }

        [BsonElement("CreatedDateTime")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDateTime { get; set; }
    }
}
