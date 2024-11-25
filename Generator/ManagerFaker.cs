using Bogus;
using Core.Models.Exp3;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class ManagerFaker: Faker<ManagerModel>
    {
        private static long id = 1;
        private string[] methods = {"Scrum","Kanban","Lean","Agile","Waterfall"};
        private string[] departments = {"HR","IT","Sales","Financial","Marketing","Logistics"};

        public ManagerFaker()
        {
            RuleFor(c => c.Id, _ => id++);
            RuleFor(c => c.FirstName, f => f.Name.FirstName());
            RuleFor(c => c.LastName, f => f.Name.LastName());
            RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));
            RuleFor(c => c.BirthDay, f => f.Date.Past(50, DateTime.Now.AddYears(-18)));
            RuleFor(c => c.Title, f => f.Name.JobTitle());
            RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"));
            RuleFor(c => c.Department, f => f.PickRandom(departments));
            RuleFor(c=>c.Method,f=>f.PickRandom(methods));
            RuleFor(c => c.RealisedProject, f => f.Random.Number(4, 20));
        }
    }
}
