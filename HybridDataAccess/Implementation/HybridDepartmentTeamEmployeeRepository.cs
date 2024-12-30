using Core.Models.Exp2;
using DataAccess;
using HelperMapper.Mappers;
using HybridDataAccess.Queries.Exp2;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridDataAccess.Implementation
{
    public sealed class HybridDepartmentTeamEmployeeRepository : HybridRepository,IDepartmentTeamEmployeeRepository
    {
        public HybridDepartmentTeamEmployeeRepository(string database) : base(database)
        {
        }

        public HybridDepartmentTeamEmployeeRepository() : base("exp2_hybrid_db")
        {
        }

        public void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment2Hybrid.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void InsertDepartmentWithTeams(DepartmentModel department)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", department.Id);
            command.Parameters.AddWithValue("@Name", department.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Location", department.Location ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TeamsWithEmployees",_jsonHandler.SerializeMany(department.Teams));

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void InsertEmployee(EmployeeModel2 employee)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Clear();
            employee.Team = null;
            command.Parameters.AddWithValue("@TeamId",employee.Id);
            command.Parameters.AddWithValue("@Employees",
                _jsonHandler.SerializeMany(new List<EmployeeModel2> { employee }));

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void InsertDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();

            foreach (var department in departments)
            {
                command.Parameters.Clear();

                command.Parameters.AddWithValue("@Id", department.Id);
                command.Parameters.AddWithValue("@Name", department.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Location", department.Location ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TeamsWithEmployees", _jsonHandler.SerializeMany(department.Teams));

                command.ExecuteNonQuery();
            }

            
        }

        public void InsertEmployees(List<EmployeeModel2> employees)
        {
            var groupedByTeam = employees
                .GroupBy(emp => emp.Team)
                .Select(group => new
                    {
                        Team = group.Key,
                        Employees = group.ToList()
                    });
            
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            foreach(var teamGroup in groupedByTeam)
            {
                foreach(var emp in teamGroup.Employees)
                {
                    emp.Team = null;
                }
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@TeamId", teamGroup.Team.Id);
                command.Parameters.AddWithValue("@Employees",_jsonHandler.SerializeMany(teamGroup.Employees));

                command.ExecuteNonQuery();
            }
        }

        public void InsertBulkDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            var dataTable = new DataTable();

            // Definišemo kolone
            dataTable.Columns.Add("id", typeof(long));
            dataTable.Columns.Add("name", typeof(string));
            dataTable.Columns.Add("location", typeof(string));
            dataTable.Columns.Add("teams", typeof(string));

            // Popunjavamo redove
            foreach (var department in departments)
            {
                dataTable.Rows.Add(
                    department.Id,
                    department.Name ?? (object)DBNull.Value,
                    department.Location ?? (object)DBNull.Value,
                    _jsonHandler.SerializeMany(department.Teams) ?? (object)DBNull.Value
                );
            }

            using var copy = new SqlBulkCopy(_connectionString);

            copy.DestinationTableName = "dbo.Department";

            copy.ColumnMappings.Add(nameof(DepartmentModel.Id), "id");
            copy.ColumnMappings.Add(nameof(DepartmentModel.Name), "name");
            copy.ColumnMappings.Add(nameof(DepartmentModel.Location), "location");
            copy.ColumnMappings.Add(nameof(DepartmentModel.Teams), "teams");

            copy.WriteToServer(dataTable);
            

        }

        public void InsertBulkEmployees(List<EmployeeModel2> employees)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetAllDepartmentsBadWay()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command= new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[0], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            List<DepartmentModel> departments = new List<DepartmentModel>();
            while (reader.Read())
            {
                DepartmentModel department = new DepartmentModel();
                department.Id = reader.GetInt64(0);
                department.Name = reader.GetString(1);
                department.Location = reader.GetString(2);
                department.Teams=_jsonHandler.DeserializeMany<TeamModel>(reader.GetString(3));

                departments.Add(department);
            }

            return departments;
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[1], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWtihTeamsAndEmployees = Exp2HelperMapper.MapDepartmentsWithAliases(reader);
            return departmentsWtihTeamsAndEmployees;
        }

        public List<EmployeeModel2> GetAllEmployees(long teamId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[3], connection);
            command.Parameters.AddWithValue("@TeamId", teamId);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employees = Exp2HelperMapper.MapEmployees(reader);
            return employees;
        }

        public List<TeamModel> GetAllTeams(long departmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[2], connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            connection.Open();
            using var reader = command.ExecuteReader();
            var teamsWithEmployees = Exp2HelperMapper.MapTeamsWithAliases(reader);
            return teamsWithEmployees;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[5], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentsWithTeams(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[6], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentsWithTeams(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[8], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentAndTeamAggregates(reader);
            return departmentsWithTeams;
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[7], connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var departmentsWithTeams = Exp2HelperMapper.MapDepartmentAndTeamAggregatesBearer(reader);
            return departmentsWithTeams;
        }

        public EmployeeModel2 GetEmployee(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Select)[4], connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employees = Exp2HelperMapper.MapEmployees(reader);
            return employees.First();
        }

        public void UpdateDescriptionComplex()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Update)[4], connection);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Update)[3], connection);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Update)[2], connection);

            command.Parameters.AddWithValue("@Description", description);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Update)[1], connection);

            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GenerateQueriesFromQuery(Experiment2Hybrid.Update)[0], connection);

            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
