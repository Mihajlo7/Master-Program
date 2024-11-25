using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public sealed class SoftwareDeveloper : DeveloperModel
    {
        public string ProgrammingLanguage { get; set; }= string .Empty;
        public string IDE {  get; set; }=string .Empty;
        public bool IsFullStack { get; set; }
    }
}
