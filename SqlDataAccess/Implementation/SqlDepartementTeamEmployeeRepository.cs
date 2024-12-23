using Core.Models.Exp2;
using DataAccess;
using HelperMapper.Mappers;
using Microsoft.Data.SqlClient;
using SqlDataAccess.Queries.Exp2;
using SqlDataAccess.Queries.Exp3;
using System;
using System.Collections.Generic;
using System.Data;
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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Kreiraj DataTable za Department
                        var departmentTable = new DataTable();
                        departmentTable.Columns.Add("id", typeof(long));
                        departmentTable.Columns.Add("name", typeof(string));
                        departmentTable.Columns.Add("location", typeof(string));

                        foreach (var dept in departments)
                        {
                            departmentTable.Rows.Add(dept.Id, dept.Name, dept.Location);
                        }

                        // Ubaci podatke u tabelu Department
                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "Department";
                            bulkCopy.WriteToServer(departmentTable);
                        }

                        // Kreiraj DataTable za Team
                        var teamTable = new DataTable();
                        teamTable.Columns.Add("id", typeof(long));
                        teamTable.Columns.Add("name", typeof(string));
                        teamTable.Columns.Add("status", typeof(string));
                        teamTable.Columns.Add("description", typeof(string));
                        teamTable.Columns.Add("department_id", typeof(long));
                        teamTable.Columns.Add("leader_id", typeof(long));

                        foreach (var dept in departments)
                        {
                            if (dept.Teams != null)
                            {
                                foreach (var team in dept.Teams)
                                {
                                    teamTable.Rows.Add(
                                        team.Id,
                                        team.Name,
                                        team.Status,
                                        team.Description,
                                        dept.Id, // department_id
                                        team.LeaderId // leader_id
                                    );
                                }
                            }
                        }

                        // Ubaci podatke u tabelu Team
                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            
                            bulkCopy.DestinationTableName = "Team";
                            bulkCopy.WriteToServer(teamTable);
                        }

                        // Commit transakciju
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public void InsertBulkEmployees(List<EmployeeModel2> employees)
        {
            var employeeTable = new DataTable();
            employeeTable.Columns.Add("id", typeof(long));
            employeeTable.Columns.Add("first_name", typeof(string));
            employeeTable.Columns.Add("last_name", typeof(string));
            employeeTable.Columns.Add("email", typeof(string));
            employeeTable.Columns.Add("birth_day", typeof(DateTime));
            employeeTable.Columns.Add("title", typeof(string));
            employeeTable.Columns.Add("phone", typeof(string));
            employeeTable.Columns.Add("team_id", typeof(long));

            foreach (var employee in employees)
            {
                employeeTable.Rows.Add(
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Email,
                    employee.BirthDay ?? (object)DBNull.Value,
                    employee.Title,
                    employee.Phone,
                    employee.TeamId // team_id
                );
            }
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "Employee";
                bulkCopy.WriteToServer(employeeTable);
            }

            // Commit transakciju
            transaction.Commit();

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

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[3], connection);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[2], connection);

            command.Parameters.AddWithValue("@Description", description);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[1], connection);

            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Sql.Update)[0],connection);

            command.Parameters.AddWithValue("@Status",status);
            command.Parameters.AddWithValue("@Id",id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
