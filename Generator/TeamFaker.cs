using Bogus;
using Core.Models.Exp2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class TeamFaker : Faker<TeamModel>
    {
        private static long id = 1;
        public TeamFaker()
        {
            RuleFor(t => t.Id,_=> id++);
            RuleFor(t => t.Name, f=>f.Company.CatchPhrase());
            RuleFor(t=>t.Description, f=>f.Lorem.Sentence(3));
            RuleFor(t=>t.Status, "Active");
        }
    }
}
