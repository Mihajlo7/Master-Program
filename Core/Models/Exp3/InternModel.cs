using Core.Models.Exp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public sealed class InternModel : EmployeeModel3
    {
        public string University { get; set; } = null;
        public int StudyYear { get; set; }
        public string City { get; set; }
    }
}
