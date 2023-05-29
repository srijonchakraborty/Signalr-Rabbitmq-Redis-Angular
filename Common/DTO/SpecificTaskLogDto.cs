using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class SpecificTaskLogDto
    {
        public string? Id { get; set; }
        public string? Description { get; set; }
        public string? SubscriptionId { get; set; }
        public string? ConnectionId { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
