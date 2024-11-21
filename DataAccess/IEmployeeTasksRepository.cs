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

        public List<TaskModel> GetAllTasksByDeadilineAndNotComplited(int day);
        public List<TaskModel> GetAllTasksByResponsibleNameAndSupervisorBirthday(string firstname,DateTime birthday);
        // Get Employee with count of tasks
        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasks();
        // Get Employee with count of tasks and order by
        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasksHavingAndOrder(int numOfTasks);
        

        // Azuriraj broj telefona za neki email
        // Azurirajte tasks tako na Task ako je isterkao 
        // Azurirajte zadatak povecaj deadline na 5 dana ako je prioritet veci od 7
        // Azurirajte zadatke povecaj deadline gde je naziv Responsible first name pocinje na A
        // Azuirajte zadatak na Canceled ako je Responisble titula sadrzi Engineer i rodjen pre 80
        // Prebacite Zadatke jednog zaposlenog na drugog zaposlenog ako je otvoren
    }
}
