using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public class EmployeeBase
    {
        public long Id { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Title {  get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
    }
}
