using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp2;

namespace DataAccess
{
    public interface IDepartmentTeamEmployeeRepository
    {
        public abstract void ExecuteCreationTable();
        public abstract void ExecuteCreationAdditional();
        public void InsertDepartmentWithTeams(DepartmentModel department);
        public void InsertEmployee(EmployeeModel2 employee);

        public void InsertDepartmenstWithTeams(List<DepartmentModel> departments);
        public void InsertEmployees(List<EmployeeModel2> employees);

        public void InsertBulkDepartmenstWithTeams(List<DepartmentModel> departments);
        public void InsertBulkEmployees(List<EmployeeModel2> employees);
        // Vrati sve timove i zaposlene po department *
        public List<DepartmentModel> GetAllDepartmentsBadWay();
        // Vrati sve timove i zaposlene po department naziv
        public List<DepartmentModel> GetAllDepartments();
        // Vrati sve po timove po department id
        public List<TeamModel> GetAllTeams(long departmentId);
        // vrati zaposlene po id tima
        public List<EmployeeModel2> GetAllEmployees(long teamId);
        // vrati zaposlenog po Id
        public EmployeeModel2 GetEmployee(int id);
        // vrati sve departmenta sa timovima koji su u status Active i sort po nazivu
        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted();
        // vrati sve department sa timovima u kojima je sef mladji od 35 g i imaju Engineer
        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer();
        // vrati department i broj zaposlenih koji su izmedju 30 i 40 godina
        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy();
        // vrati za svaki departemnt vrati timove koje broje zaposlene koji su Engineer i ima vise od 40 i sort
        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy();

        public void UpdateStatusTeam(long id,string status);
        public void UpdateEmployeePhone(long id,string phone);
        public void UpdateDescriptionTeamsFromPrague(string description);
        public void UpdateDescriptionTeamsForYoungEmployees();
        public void UpdateDescriptionComplex();
    }
}
