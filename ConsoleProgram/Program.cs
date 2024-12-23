using BenchmarkDotNet.Running;
using ConsoleProgram.Benchmark.Exp1;
using ConsoleProgram.Benchmark.Exp2;
using ConsoleProgram.Benchmark.Exp3;
using ConsoleProgram.Generator;
using ConsoleProgram.Setup;
using Core.Models.Exp1;
using Core.Models.Exp2;
using Core.Models.Exp3;
using Generator;
using HybridDataAccess.DataSerializator;
using HybridDataAccess.Implementation;
using Microsoft.IdentityModel.JsonWebTokens;
using MongoDataAccess;
using MongoDataAccess.Implementation;
using SqlDataAccess.Implementation;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using static BenchmarkDotNet.Engines.EngineEventSource;

namespace ConsoleProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Experiment2Benchmark>();


            
            GeneratorService generatorService = new GeneratorService();
            JsonHandler jsonHandler = new JsonHandler();
            var (departments, employees) = generatorService.GenerateDepartmentsAndEmployeers(20,1250);
            var employeesRepository = new HybridDepartmentTeamEmployeeRepository();
            employeesRepository.ExecuteCreationTable();
            employeesRepository.InsertBulkDepartmenstWithTeams(departments);
            employeesRepository.InsertEmployees(employees);
            Console.WriteLine("**********************************************************");
            
            
            //Console.WriteLine(jsonHandler.SerializeMany<TaskModel>(tasks));
            /*
            HybridEmployeeRepository sqlEmployeeRepository = new HybridEmployeeRepository();
            
            Console.WriteLine("Creating tables ...");
            sqlEmployeeRepository.ExecuteCreationTable();
            Console.WriteLine("Generating data ...");
            var (menagers, developers) = generatorService.GenerateDataManagersAndDevelopers(5000);
            Console.WriteLine("Inserting managers ...");
            sqlEmployeeRepository.InsertManyManager(menagers);
            Console.WriteLine("Inserting developers ...");
            sqlEmployeeRepository.InsertManySoftwareDeveloper(developers);
            */

            //Console.WriteLine("Generating data ...");
            //var (managers, softDevs) = generatorService.GenerateDataManagersAndDevelopers(5000);
            //MongoDepartmentTeamEmployeeRepository sql = new();
            //Console.WriteLine("Creating tables ...");
            //sql.ExecuteCreationTable();
            //Console.WriteLine("Inserting data ...");
            //sql.InsertManyManager(managers);
            //sql.InsertManySoftwareDeveloper(softDevs);
            //Console.WriteLine("Complited");
            //sql.InsertEmployees(employees);

            //Console.WriteLine("Reading data ...");
            //var departmentsRes = sql.UpdatePhoneById();
            //sql.UpdateDescriptionTeamsForYoungEmployees();
            //Console.WriteLine("Writting data ...");
            //string json= jsonHandler.SerializeMany(departmentsRes);
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine(json);

        }
    }
}
