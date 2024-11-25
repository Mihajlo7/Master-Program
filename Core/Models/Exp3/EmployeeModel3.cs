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
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
