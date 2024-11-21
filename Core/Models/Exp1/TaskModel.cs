using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.Base;

namespace Core.Models.Exp1
{
    public sealed class TaskModel : TaskBase
    {

        public EmloyeeModel? Responsible { get; set; } = null;
        public EmloyeeModel? Supervisor { get; set; }= null;
        
        public List<EmployeeTaskModel>? Employees { get; set; }= null;

        public TaskModel()
        {
            
        }
        public TaskModel(TaskBase taskBase)
        {
            Id = taskBase.Id;
            Name = taskBase.Name;
            Deadline = taskBase.Deadline;
            Description = taskBase.Description;
            Status = taskBase.Status;
            Priority = taskBase.Priority;
        }
    }
}
