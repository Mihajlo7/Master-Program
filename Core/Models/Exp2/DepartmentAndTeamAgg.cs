using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp2
{
    public class DepartmentAndTeamAgg
    {
        
        public long DepartmentId { get; set; }
        public string? DepartmentName { get; set; } = null;
        public long? TeamId { get; set; } = null;
        public string? TeamName { get; set;} = null;
        public int EmployeesCount { get; set; }
    }
}
