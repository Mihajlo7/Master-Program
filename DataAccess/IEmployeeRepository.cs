using Core.Models.Exp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IEmployeeRepository
    {
        public abstract void ExecuteCreationTable();
        public abstract void ExecuteCreationAdditional();
        public void InsertManager(ManagerModel manager);
        public void InsertSoftwareDeveloper(SoftwareDeveloperModel softwareDeveloper);
        public void InsertManyManager(List<ManagerModel> managers);
        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers);
        public void InsertBulkManager(List<ManagerModel> managers);
        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers);
    }
}
