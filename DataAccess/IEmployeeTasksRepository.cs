using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IEmployeeTasksRepository 
    {
        public void InsertOne();
        public int InsertMany();
        public void InsertBulk();

        // Get all with *
        // Get all but with names and join
        // Get all but with names and subquery
        // Get Task by Id 
        // Get Task by Priority and Status
        // Get Employee with count of tasks
        // Get Employee with count of tasks and order by
        // Get tasks and employees with deadline of 7 days

        // Azuriraj broj telefona za neki email
        // Azurirajte tasks tako na Task ako je isterkao i ako je Pending
        // Azurirajte zadatak povecaj deadline na 5 dana ako je prioritet veci od 7
        // Prebacite Zadatke jednog zaposlenog na drugog zaposlenog ako je otvoren
    }
}
