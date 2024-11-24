using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp1
{
    public sealed class EmployeeTaskModel
    {
        public long? EmployeeId { get; set; } = null;
        public long? TaskId { get; set; }= null;
        public EmployeeModel? Employee { get; set; }=null;
        public TaskModel? Task { get; set; } = null;
    }
}
