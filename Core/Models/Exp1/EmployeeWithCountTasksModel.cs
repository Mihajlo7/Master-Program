using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp1
{
    public sealed class EmployeeWithCountTasksModel
    {
        public long Id { get; set; }
        public string Email { get; set; }=string.Empty;
        public int Count { get; set; }
    }
}
