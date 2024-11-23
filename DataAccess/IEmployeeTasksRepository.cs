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
        public abstract void ExecuteCreationTable();
        public abstract void ExecuteCreationAdditional();
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


        public int UpdateExpiredTaskByDeadline();
        public int UpdatePhoneById(string phone,long id);
        public int UpdatePhoneByEmail(string phone, string email);
        public int UpdateDeadlineByPriorityByDeadline(int priority, int day);
        public int UpdateDeadlineByResponsibleLastName(string lastName);
        public int UpdateDeadlineByResponsibleTitleAndBirthday();
        public int UpdateTasksFromOneEmployeeToOther(long fromEmployee,long toEmployee);

        public bool DeleteAllTasks();
        public bool DeleteTaskById(long id);
        public bool DeleteTasksByStatus(string status);
        public bool DeleteTasksByResponsibleId(long employeeId);
        public bool DeleteTasksBySupervisorFirstName();
    }
}
