using Bogus;
using Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class EmployeeFaker : Faker<EmployeeBase>
    {
        private static long idEmployee = 1;
        public EmployeeFaker() 
        {
            RuleFor(c => c.Id, _ => idEmployee++); 
            RuleFor(c => c.FirstName, f => f.Name.FirstName());
            RuleFor(c => c.LastName, f => f.Name.LastName());
            RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));
            RuleFor(c => c.BirthDay, f => f.Date.Past(50, DateTime.Now.AddYears(-18)));
            RuleFor(c => c.Title, f => f.Name.JobTitle());
            RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"));
        }
    }
}
