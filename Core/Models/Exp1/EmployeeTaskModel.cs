﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp1
{
    public sealed class EmployeeTaskModel
    {
        public long EmloyeeId { get; set; }
        public long TaskId { get; set; }
        public EmloyeeModel? Emloyee { get; set; }
        public TaskModel? Task { get; set; }
    }
}
