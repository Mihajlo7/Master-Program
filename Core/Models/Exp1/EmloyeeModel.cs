using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;

namespace Core.Models.Exp1
{
    public sealed class EmloyeeModel : EmployeeBase
    {
        public EmloyeeModel(EmployeeBase employeeBase)
        {
            Id = employeeBase.Id;
            Email = employeeBase.Email;
            FirstName = employeeBase.FirstName;
            LastName = employeeBase.LastName;
            Phone = employeeBase.Phone;
            Title = employeeBase.Title;
            BirthDay = employeeBase.BirthDay;
        }

        public EmloyeeModel()
        {
            
        }
    }
}
