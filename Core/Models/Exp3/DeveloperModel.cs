using Core.Models.Exp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public class DeveloperModel : EmployeeModel3
    {
        public string Seniority { get; set; }= string.Empty;
        public int YearsOfExperience { get; set; }
        public bool IsRemote { get; set; }
    }
}
