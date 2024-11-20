using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;

namespace Core.Models.Exp1
{
    public sealed class TaskModel : TaskBase
    {
        public EmloyeeModel Responsible { get; set; } = new();
        public EmloyeeModel? Supervisor { get; set; }
        public List<EmployeeTaskModel> Employees { get; set; }= new();
    }
}
