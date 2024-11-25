using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp3
{
    public sealed class DatabaseDeveloperModel : DeveloperModel
    {
        public string Provider {  get; set; }=string.Empty;
        public bool IsAdmin { get; set; }
        public bool KnowNoSql { get; set; }
    }
}
