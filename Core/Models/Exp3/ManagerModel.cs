using Core.Models.Exp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public sealed class ManagerModel : EmployeeModel3
    {
        public string Department { get; set; } = string.Empty;
        public int RealisedProject { get; set; }
        public string Method {  get; set; } = string.Empty;
    }
}
