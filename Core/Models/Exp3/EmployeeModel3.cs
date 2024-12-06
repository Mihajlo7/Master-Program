using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public class EmployeeModel3
    {
        [BsonId]
        [BsonElement("_id")]
        public long Id { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
        public DateTime? BirthDay { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Phone { get; set; } = null;
        public string ? Role { get; set; } = null;
    }
}
