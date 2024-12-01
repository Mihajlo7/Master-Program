using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp2;
using Microsoft.Data.SqlClient;

namespace HelperMapper.Mappers
{
    public static class Exp2HelperMapper
    {
        public static List<DepartmentModel> MapDepartmentsWithIndexes(SqlDataReader reader)
        {
            var departments = new Dictionary<long, DepartmentModel>();
            var teams = new Dictionary<long, TeamModel>();

            while (reader.Read())
            {
                // Mapiranje Department
                var departmentId = reader.GetInt64(0); // Pretpostavimo da je prva kolona `id` iz tabele `Department`
                if (!departments.ContainsKey(departmentId))
                {
                    departments[departmentId] = new DepartmentModel
                    {
                        Id = departmentId,
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1), // Druga kolona je `name`
                        Location = reader.IsDBNull(2) ? null : reader.GetString(2), // Treća kolona je `location`
                        Teams = new List<TeamModel>()
                    };
                }

                var department = departments[departmentId];

                // Mapiranje Team
                if (!reader.IsDBNull(3)) // Četvrta kolona je `team.id`
                {
                    var teamId = reader.GetInt64(3);
                    if (!teams.ContainsKey(teamId))
                    {
                        var nteam = new TeamModel
                        {
                            Id = teamId,
                            Name = reader.IsDBNull(4) ? null : reader.GetString(4), // Peta kolona je `team.name`
                            Status = reader.IsDBNull(5) ? null : reader.GetString(5), // Šesta kolona je `team.status`
                            Description = reader.IsDBNull(6) ? null : reader.GetString(6), // Sedma kolona je `team.description`
                            //Department = department,
                            LeaderId = reader.IsDBNull(8) ? (long?)null : reader.GetInt64(8) // Osma kolona je `team.leader_id`
                        };
                        teams[teamId] = nteam;
                        department.Teams.Add(nteam);
                    }

                    var team = teams[teamId];

                    // Mapiranje Lead (lider tima)
                    if (!reader.IsDBNull(8)) // Deveta kolona je `employee.id` lidera
                    {
                        team.Lead = new EmployeeModel2
                        {
                            Id = reader.GetInt64(8),
                            FirstName = reader.IsDBNull(10) ? null : reader.GetString(10), // Deseta kolona je `employee.first_name`
                            LastName = reader.IsDBNull(11) ? null : reader.GetString(11), // Jedanaesta kolona je `employee.last_name`
                            Email = reader.IsDBNull(12) ? null : reader.GetString(12), // Dvanaesta kolona je `employee.email`
                            BirthDay = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13), // Trinaesta kolona je `employee.birth_day`
                            Title = reader.IsDBNull(14) ? null : reader.GetString(14), // Četrnaesta kolona je `employee.title`
                            Phone = reader.IsDBNull(15) ? null : reader.GetString(15), // Petnaesta kolona je `employee.phone`
                            //Team = team
                        };
                    }

                    // Mapiranje Employees (članovi tima)
                    if (!reader.IsDBNull(17)) // Šesnaesta kolona je `employee.id` člana tima
                    {
                        var employee = new EmployeeModel2
                        {
                            Id = reader.GetInt64(17),
                            FirstName = reader.IsDBNull(18) ? null : reader.GetString(18), // Sedamnaesta kolona
                            LastName = reader.IsDBNull(19) ? null : reader.GetString(19), // Osamnaesta kolona
                            Email = reader.IsDBNull(20) ? null : reader.GetString(20), // Devetnaesta kolona
                            BirthDay = reader.IsDBNull(21) ? (DateTime?)null : reader.GetDateTime(21), // Dvadeseta kolona
                            Title = reader.IsDBNull(22) ? null : reader.GetString(22), // Dvadesetprva kolona
                            Phone = reader.IsDBNull(23) ? null : reader.GetString(23), // Dvadesetdruga kolona
                            //Team = team
                        };

                        if (team.Employees == null)
                        {
                            team.Employees = new List<EmployeeModel2>();
                        }

                        team.Employees.Add(employee);
                    }
                }
            }

            return departments.Values.ToList();
        }

        public static List<DepartmentModel> MapDepartmentsWithAliases(SqlDataReader reader)
        {
            var departments = new Dictionary<long, DepartmentModel>();
            var teams = new Dictionary<long, TeamModel>();

            while (reader.Read())
            {
                // Mapiranje Department
                var departmentId = reader.GetInt64(reader.GetOrdinal("DepartementId"));
                if (!departments.ContainsKey(departmentId))
                {
                    departments[departmentId] = new DepartmentModel
                    {
                        Id = departmentId,
                        Name = reader.IsDBNull(reader.GetOrdinal("DepartmentName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Location = reader.IsDBNull(reader.GetOrdinal("DepartmentLocation"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("DepartmentLocation")),
                        Teams = new List<TeamModel>()
                    };
                }

                var department = departments[departmentId];

                // Mapiranje Team
                if (!reader.IsDBNull(reader.GetOrdinal("TeamId")))
                {
                    var teamId = reader.GetInt64(reader.GetOrdinal("TeamId"));
                    if (!teams.ContainsKey(teamId))
                    {
                        var nteam = new TeamModel
                        {
                            Id = teamId,
                            Name = reader.IsDBNull(reader.GetOrdinal("TeamName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("TeamName")),
                            Status = reader.IsDBNull(reader.GetOrdinal("TeamStatus"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("TeamStatus")),
                            Description = reader.IsDBNull(reader.GetOrdinal("TeamDescription"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("TeamDescription")),
                            //Department = department,
                            LeaderId = reader.IsDBNull(reader.GetOrdinal("LeadId"))
                                ? (long?)null
                                : reader.GetInt64(reader.GetOrdinal("LeadId"))
                        };
                        teams[teamId] = nteam;
                        department.Teams.Add(nteam);
                    }

                    var team = teams[teamId];

                    // Mapiranje Lead (lider tima)
                    if (!reader.IsDBNull(reader.GetOrdinal("LeadId")))
                    {
                        team.Lead = new EmployeeModel2
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("LeadId")),
                            FirstName = reader.IsDBNull(reader.GetOrdinal("LeadFirstName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("LeadFirstName")),
                            LastName = reader.IsDBNull(reader.GetOrdinal("LeadLastName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("LeadLastName")),
                            Email = reader.IsDBNull(reader.GetOrdinal("LeadEmail"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("LeadEmail")),
                            BirthDay = reader.IsDBNull(reader.GetOrdinal("LeadBirthDay"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("LeadBirthDay")),
                            Title = reader.IsDBNull(reader.GetOrdinal("LeadTitle"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("LeadTitle")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("LeadPhone"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("LeadPhone")),
                            //Team = team
                        };
                    }

                    // Mapiranje Employees (članovi tima)
                    if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                    {
                        var employee = new EmployeeModel2
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("Id")),
                            FirstName = reader.IsDBNull(reader.GetOrdinal("Firstname"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Firstname")),
                            LastName = reader.IsDBNull(reader.GetOrdinal("Lastname"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Lastname")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Email")),
                            BirthDay = reader.IsDBNull(reader.GetOrdinal("BirthDay"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("BirthDay")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Title")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Phone")),
                            //Team = team
                        };

                        if (team.Employees == null)
                        {
                            team.Employees = new List<EmployeeModel2>();
                        }

                        team.Employees.Add(employee);
                    }
                }
            }

            return departments.Values.ToList();
        }

        public static List<TeamModel> MapTeamsWithAliases(SqlDataReader reader)
        {
            var teams = new Dictionary<long, TeamModel>();

            while (reader.Read())
            {
                // Mapiranje Team
                var teamId = reader.GetInt64(reader.GetOrdinal("TeamId"));
                if (!teams.ContainsKey(teamId))
                {
                    teams[teamId] = new TeamModel
                    {
                        Id = teamId,
                        Name = reader.IsDBNull(reader.GetOrdinal("TeamName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamName")),
                        Status = reader.IsDBNull(reader.GetOrdinal("TeamStatus"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamStatus")),
                        Description = reader.IsDBNull(reader.GetOrdinal("TeamDescription"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamDescription")),
                        Lead = null,
                        Employees = new List<EmployeeModel2>()
                    };
                }

                var team = teams[teamId];

                // Mapiranje Lead (lider tima)
                if (!reader.IsDBNull(reader.GetOrdinal("LeadId")))
                {
                    team.Lead = new EmployeeModel2
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("LeadId")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("LeadFirstName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("LeadFirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LeadLastName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("LeadLastName")),
                        Email = reader.IsDBNull(reader.GetOrdinal("LeadEmail"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("LeadEmail")),
                        BirthDay = reader.IsDBNull(reader.GetOrdinal("LeadBirthDay"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("LeadBirthDay")),
                        Title = reader.IsDBNull(reader.GetOrdinal("LeadTitle"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("LeadTitle")),
                        Phone = reader.IsDBNull(reader.GetOrdinal("LeadPhone"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("LeadPhone"))
                    };
                }

                // Mapiranje Employees (članovi tima)
                if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                {
                    var employee = new EmployeeModel2
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("Id")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("Firstname"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Firstname")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("Lastname"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Lastname")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Email")),
                        BirthDay = reader.IsDBNull(reader.GetOrdinal("BirthDay"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDay")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Title")),
                        Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Phone")),
                        //Team = team
                    };

                    team.Employees?.Add(employee);
                }
            }

            return teams.Values.ToList();
        }
        public static List<EmployeeModel2> MapEmployees(SqlDataReader reader)
        {
            var employees = new List<EmployeeModel2>();

            while (reader.Read())
            {
                var employee = new EmployeeModel2
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    FirstName = reader.IsDBNull(reader.GetOrdinal("Firstname"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Firstname")),
                    LastName = reader.IsDBNull(reader.GetOrdinal("Lastname"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Lastname")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Email")),
                    BirthDay = reader.IsDBNull(reader.GetOrdinal("BirthDay"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("BirthDay")),
                    Title = reader.IsDBNull(reader.GetOrdinal("Title"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Title")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Phone"))
                };

                employees.Add(employee);
            }

            return employees;
        }
        public static List<DepartmentModel> MapDepartmentsWithTeams(SqlDataReader reader)
        {
            var departments = new List<DepartmentModel>();

            while (reader.Read())
            {
                // Mapiranje osnovnih podataka o department-u
                var departmentId = reader.GetInt64(reader.GetOrdinal("DepartementId"));
                var department = departments.FirstOrDefault(d => d.Id == departmentId);
                if (department == null)
                {
                    department = new DepartmentModel
                    {
                        Id = departmentId,
                        Name = reader.IsDBNull(reader.GetOrdinal("DepartmentName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Location = reader.IsDBNull(reader.GetOrdinal("DepartmentLocation"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("DepartmentLocation")),
                        Teams = new List<TeamModel>()
                    };
                    departments.Add(department);
                }

                // Mapiranje podataka o timu (ako postoji)
                if (!reader.IsDBNull(reader.GetOrdinal("TeamId")))
                {
                    var team = new TeamModel
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("TeamId")),
                        Name = reader.IsDBNull(reader.GetOrdinal("TeamName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamName")),
                        Status = reader.IsDBNull(reader.GetOrdinal("TeamStatus"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamStatus")),
                        Description = reader.IsDBNull(reader.GetOrdinal("TeamDescription"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TeamDescription"))
                    };
                    department.Teams?.Add(team);
                }
            }

            return departments;
        }

        public static List<DepartmentAndTeamAgg> MapDepartmentAndTeamAggregates(SqlDataReader reader)
        {
            var result = new List<DepartmentAndTeamAgg>();

            while (reader.Read())
            {
                var record = new DepartmentAndTeamAgg
                {
                    DepartmentId = reader.GetInt64(reader.GetOrdinal("DepartmentId")),

                    DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DepartmentName")),

                    TeamId = reader.IsDBNull(reader.GetOrdinal("TeamId"))
                        ? (long?)null
                        : reader.GetInt64(reader.GetOrdinal("TeamId")),
                    TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("TeamName")),
                    EmployeesCount = reader.GetInt32(reader.GetOrdinal("EmployeesCount"))
                };

                result.Add(record);
            }

            return result;
        }
        public static List<DepartmentAndTeamAgg> MapDepartmentAndTeamAggregatesBearer(SqlDataReader reader)
        {
            var result = new List<DepartmentAndTeamAgg>();

            while (reader.Read())
            {
                var record = new DepartmentAndTeamAgg
                {
                    DepartmentId = reader.GetInt64(reader.GetOrdinal("DepartmentId")),

                    DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DepartmentName")),

                    EmployeesCount = reader.GetInt32(reader.GetOrdinal("EmployeesCount"))
                };

                result.Add(record);
            }

            return result;
        }
        public static DataTable CreateDepartmentDataTable(IEnumerable<DepartmentModel> departments)
        {
            var dataTable = new DataTable("Department");

            // Definišemo kolone
            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Location", typeof(string));

            // Popunjavamo redove
            foreach (var department in departments)
            {
                dataTable.Rows.Add(
                    department.Id,
                    department.Name ?? (object)DBNull.Value,
                    department.Location ?? (object)DBNull.Value
                );
            }

            return dataTable;
        }

        public static DataTable CreateEmployeeDataTable(IEnumerable<EmployeeModel2> employees)
        {
            var dataTable = new DataTable("Employee");

            // Definišemo kolone
            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("FirstName", typeof(string));
            dataTable.Columns.Add("LastName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("BirthDay", typeof(DateTime));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Phone", typeof(string));

            // Popunjavamo redove
            foreach (var employee in employees)
            {
                dataTable.Rows.Add(
                    employee.Id,
                    employee.FirstName ?? (object)DBNull.Value,
                    employee.LastName ?? (object)DBNull.Value,
                    employee.Email ?? (object)DBNull.Value,
                    employee.BirthDay ?? (object)DBNull.Value,
                    employee.Title ?? (object)DBNull.Value,
                    employee.Phone ?? (object)DBNull.Value
                );
            }

            return dataTable;
        }

        public static DataTable CreateTeamDataTable(IEnumerable<TeamModel> teams)
        {
            var dataTable = new DataTable("Team");

            // Definišemo kolone
            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("DepartmentId", typeof(long));
            dataTable.Columns.Add("LeaderId", typeof(long));

            // Popunjavamo redove
            foreach (var team in teams)
            {
                dataTable.Rows.Add(
                    team.Id,
                    team.Name ?? (object)DBNull.Value,
                    team.Status ?? (object)DBNull.Value,
                    team.Description ?? (object)DBNull.Value,
                    team.Department?.Id ?? (object)DBNull.Value,
                    team.LeaderId ?? (object)DBNull.Value
                );
            }

            return dataTable;
        }

        public static SqlCommand CreateCommandEmployee2(this SqlCommand command,EmployeeModel2 employee)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", employee.Id);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LastName", employee.LastName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@BirthDay", employee.BirthDay ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Title", employee.Title ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", employee.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TeamId",employee.Team?.Id ?? (object)DBNull.Value);

            return command;
        }

        public static SqlCommand CreateCommandDepartment(this SqlCommand command,DepartmentModel department)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", department.Id);
            command.Parameters.AddWithValue("@Name", department.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Location", department.Location ?? (object)DBNull.Value);
            return command;
        }

        public static SqlCommand CreateCommandTeam(this SqlCommand command,TeamModel team)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", team.Id);
            command.Parameters.AddWithValue("@Name", team.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Status", team.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Description", team.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DepartmentId", team.Department?.Id ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LeaderId", team.LeaderId ?? (object)DBNull.Value);

            return command;
        }

        
    }

}
