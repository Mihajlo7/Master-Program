using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Exp2
{
    public sealed class DepartmentModel
    {
        public long Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Location { get; set; } = null;
        public List<TeamModel>? Teams { get; set; } = null;
    }
}
