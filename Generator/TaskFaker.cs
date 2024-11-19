using Bogus;
using Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public sealed class TaskFaker : Faker<TaskBase>
    {
        private static long id = 1;
        private readonly string[] STATUSES = {"New","Pending","Completed","Cancelled"};
        public TaskFaker() 
        {
            RuleFor(t => t.Id,f=> id++);
            RuleFor(t => t.Name, f => f.Lorem.Sentence(3)); 
            RuleFor(t => t.Description, f => f.Lorem.Paragraph()); 
            RuleFor(t => t.Priority, f => f.Random.Number(1, 5)); 
            RuleFor(t => t.Deadline, f => f.Date.Future(1)); 
            RuleFor(t => t.Status, f => f.PickRandom(STATUSES));

        }
    }
}
