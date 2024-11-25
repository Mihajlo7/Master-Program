using Bogus;
using Core.Models.Exp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class SoftwareDeveloperFaker : Faker<SoftwareDeveloperModel>
    {
        private static long id = 1;
        private string[] seniority = { "Junior", "Medior", "Senior" };
        private string[] pls = {"C#","Java","PHP","Python","Ruby","Go" };
        private string[] ides = {"Visual Studio","Visual Studio Code","Intelij","Web Storm","Eclipse"};

        public SoftwareDeveloperFaker()
        {
            RuleFor(c => c.Id, _ => id++);
            RuleFor(c => c.FirstName, f => f.Name.FirstName());
            RuleFor(c => c.LastName, f => f.Name.LastName());
            RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));
            RuleFor(c => c.BirthDay, f => f.Date.Past(50, DateTime.Now.AddYears(-18)));
            RuleFor(c => c.Title, f => f.Name.JobTitle());
            RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"));
            RuleFor(c => c.Seniority, f => f.PickRandom(seniority));
            RuleFor(c => c.YearsOfExperience, f => f.Random.Number(1,30));
            RuleFor(c => c.IsRemote, f => f.Random.Bool());
            RuleFor(c => c.ProgrammingLanguage, f => f.PickRandom(pls));
            RuleFor(c => c.IDE, f => f.PickRandom(ides));
            RuleFor(c => c.IsFullStack, f => f.Random.Bool());
        }
    }
}
