using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models.Exp2
{
    public sealed class EmployeeModel2
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

        public TeamModel? Team { get; set; } = null;
    }
}
