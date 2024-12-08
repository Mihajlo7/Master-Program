using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp2
{
    public sealed class TeamModel
    {
        [BsonId]
        [BsonElement("_id")]
        public long Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? Description { get; set; }=null;

        public DepartmentModel? Department { get; set; } = null;
        public long? LeaderId { get; set; } = null;
        public EmployeeModel2? Lead { get; set; } = null;
        public List<EmployeeModel2>? Employees { get; set; } = null;
    }
}
