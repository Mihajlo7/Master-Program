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
            
            BenchmarkRunner.Run<Experiment3Benchmark>();

            /*
            Console.WriteLine("START");
            GeneratorService generatorService = new GeneratorService();
            JsonHandler jsonHandler = new JsonHandler();
            Console.WriteLine("GENERATING DATA ...");
            var(departments, employees) =
              generatorService.GenerateDepartmentsAndEmployeers(20, 1250);
            Console.WriteLine("DATA GENERATED");
            var employeesRepository = new MongoDepartmentTeamEmployeeRepository();
            Console.WriteLine("CLEANING DATA ...");
            employeesRepository.ExecuteCreationTable();
            Console.WriteLine("DEPARTMNETS ...");
            employeesRepository.InsertDepartmenstWithTeams(departments);
            Console.WriteLine("EMPLOYEES ...");
            Console.WriteLine("**********************************************************");
            const int size = 10_000;
            for(int i = 0;i< 50;i++)
            {
                Console.WriteLine($"Iteration: {i}");
                employeesRepository = new();
                employeesRepository.InsertEmployees(employees.GetRange(i*size,size-1));
                Console.WriteLine("Inserted 10_000 employees");
            }
            Console.WriteLine("END");
            Console.WriteLine("**********************************************************");
            */
            
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
