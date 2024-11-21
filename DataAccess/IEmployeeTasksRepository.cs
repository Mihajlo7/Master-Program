using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;

namespace DataAccess
{
    public interface IEmployeeTasksRepository 
    {
        public void InsertOne(TaskModel newTask);
        public int InsertMany(List<TaskModel> tasks);
        public void InsertBulk(List<TaskModel> tasks);

        public void InsertEmployeeBulk(List<EmloyeeModel> emloyees);

        // Get all with *
        public List<TaskModel> GetAllTasksWithEmployeesBadWay();
        // Get all but with names and join
        public List<TaskModel> GetAllTasksWithEmployees();
        // Get all but with names and ordered
        public List<TaskModel> GetAllTasksWithEmployeesSorted();
        // Get Task by Id 
        public TaskModel GetTaskWithEmployeesById(long id);
        // Get Task by Priority and Status
        public List<TaskModel> GetAllTasksWithEmployeesByPriorityAndStatus(int priority);
        // Get Employee with count of tasks
        // Get Employee with count of tasks and order by
        // Get tasks and employees with deadline of 7 days

        // Azuriraj broj telefona za neki email
        // Azurirajte tasks tako na Task ako je isterkao i ako je Pending
        // Azurirajte zadatak povecaj deadline na 5 dana ako je prioritet veci od 7
        // Prebacite Zadatke jednog zaposlenog na drugog zaposlenog ako je otvoren
    }
}
