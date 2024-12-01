using Core.Models.Exp2;
using DataAccess;
using HelperMapper.Mappers;
using Microsoft.Data.SqlClient;
using SqlDataAccess.Queries.Exp2;
using SqlDataAccess.Queries.Exp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccess.Implementation
{
    public sealed class SqlDepartementTeamEmployeeRepository : SqlRepository, IDepartmentTeamEmployeeRepository
    {
        public SqlDepartementTeamEmployeeRepository() : base("exp2_db")
        {
        }
        public SqlDepartementTeamEmployeeRepository(string database) : base(database)
        {
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment2Sql.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[1], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWtihTeamsAndEmployees = Exp2HelperMapper.MapDepartmentsWithAliases(reader);
            return departmentsWtihTeamsAndEmployees;
        }

        public List<DepartmentModel> GetAllDepartmentsBadWay()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[0],connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWtihTeamsAndEmployees=Exp2HelperMapper.MapDepartmentsWithIndexes(reader);
            return departmentsWtihTeamsAndEmployees;
        }

        public List<EmployeeModel2> GetAllEmployees(long teamId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[3], connection);
            command.Parameters.AddWithValue("@TeamId",teamId);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employees = Exp2HelperMapper.MapEmployees(reader);
            return employees;
        }

        public List<TeamModel> GetAllTeams(long departmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[2], connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            connection.Open();
            using var reader = command.ExecuteReader();
            var teamsWithEmployees = Exp2HelperMapper.MapTeamsWithAliases(reader);
            return teamsWithEmployees;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[5], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentsWithTeams(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[6], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentsWithTeams(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[8], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentAndTeamAggregates(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[7], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentAndTeamAggregatesBearer(reader);
            return departmentsWithTeams;
        }

        public EmployeeModel2 GetEmployee(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Select)[4], connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employees = Exp2HelperMapper.MapEmployees(reader);
            return employees.First();
        }

        public void InsertBulkDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            var teams= departments.SelectMany(d=>d.Teams).ToList();

            InsertBulkPriv("Department", Exp2HelperMapper.CreateDepartmentDataTable(departments));
            InsertBulkPriv("Team", Exp2HelperMapper.CreateTeamDataTable(teams));
        }

        public void InsertBulkEmployees(List<EmployeeModel2> employees)
        {
            InsertBulkPriv("Employees",Exp2HelperMapper.CreateEmployeeDataTable(employees));
        }

        public void InsertDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            string departmentQuery = GenerateQueriesFromQuery(Experiment2Sql.Insert)[0];
            string teamQuery = GenerateQueriesFromQuery(Experiment2Sql.Insert)[2];

            using var connection = new SqlConnection(_connectionString);
            var departmentCommand = new SqlCommand(departmentQuery, connection);
            var teamCommand = new SqlCommand(teamQuery, connection);
            connection.Open();
            foreach (var department in departments)
            {
                departmentCommand.CreateCommandDepartment(department);
                            departmentCommand.ExecuteNonQuery();
                            for (int i = 0; i < department.Teams.Count; i++)
                            {
                                teamCommand.CreateCommandTeam(department.Teams[i]);
                                teamCommand.ExecuteNonQuery();
                            }
            }
            
        }

        public void InsertDepartmentWithTeams(DepartmentModel department)
        {
            string departmentQuery = GenerateQueriesFromQuery(Experiment2Sql.Insert)[0];
            string teamQuery = GenerateQueriesFromQuery(Experiment2Sql.Insert)[2];

            using var connection = new SqlConnection(_connectionString);
            var departmentCommand= new SqlCommand(departmentQuery, connection);
            departmentCommand.CreateCommandDepartment(department);
            var teamCommand = new SqlCommand(teamQuery, connection);
            connection.Open();
            departmentCommand.ExecuteNonQuery();
            for(int i = 0; i < department.Teams.Count; i++)
            {
                teamCommand.CreateCommandTeam(department.Teams[i]);
                teamCommand.ExecuteNonQuery();
            }

        }

        public void InsertEmployee(EmployeeModel2 employee)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Insert)[1], connection);

            command.CreateCommandEmployee2(employee);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void InsertEmployees(List<EmployeeModel2> employees)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Insert)[1], connection);
            connection.Open();

            foreach (EmployeeModel2 employee in employees) 
            { command.CreateCommandEmployee2(employee); command.ExecuteNonQuery();}
        }

        public void UpdateDescriptionComplex()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[4], connection);


            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[3], connection);


            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[2], connection);

            command.Parameters.AddWithValue("@Description", description);

            command.ExecuteNonQuery();
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[1], connection);

            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[0],connection);

            command.Parameters.AddWithValue("@Status",status);
            command.Parameters.AddWithValue("@Id",id);

            command.ExecuteNonQuery();
        }
    }
}
