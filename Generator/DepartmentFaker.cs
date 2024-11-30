using Bogus;
using Core.Models.Exp2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class DepartmentFaker : Faker<DepartmentModel>
    {
        string[] departments = new string[]
        {
            "Human Resources",
            "Finance",
            "Marketing",
            "Sales",
            "IT",
            "Customer Service",
            "Logistics",
            "Operations",
            "Legal",
            "Research and Development",
            "Procurement",
            "Quality Assurance",
            "Public Relations",
            "Administration",
            "Engineering",
            "Product Development",
            "Training and Development",
            "Health and Safety",
            "Facilities Management",
            "Data Analytics"
        };
        string[] cities = { "Belgrade","Novi Sad","Berlin","London","Prague"};
        private static long id = 1;
        private static int i = 0;
        public DepartmentFaker() 
        {
            RuleFor(d => d.Name, _ => departments[i++]);
            RuleFor(d=>d.Id,_=> id++);
            RuleFor(d => d.Location, f => f.PickRandom(cities));

        }
    }
}
