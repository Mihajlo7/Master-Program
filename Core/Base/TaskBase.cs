﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public class TaskBase
    {
        public long Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }=DateTime.Now;
        public string Status {  get; set; } = "Unknown";
    }
}
