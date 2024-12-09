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
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GeneratorService generatorService = new GeneratorService();
            JsonHandler jsonHandler = new JsonHandler();
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
            
            Console.WriteLine("Generating data ...");
            var (managers, softDevs) = generatorService.GenerateDataManagersAndDevelopers(5000);
            MongoDepartmentTeamEmployeeRepository sql = new();
            //Console.WriteLine("Creating tables ...");
            //sql.ExecuteCreationTable();
            //Console.WriteLine("Inserting data ...");
            //sql.InsertManyManager(managers);
            //sql.InsertManySoftwareDeveloper(softDevs);
            //Console.WriteLine("Complited");
            //sql.InsertEmployees(employees);
            
            Console.WriteLine("Reading data ...");
            //var departmentsRes = sql.UpdatePhoneById();
            sql.UpdateDescriptionTeamsForYoungEmployees();
            Console.WriteLine("Writting data ...");
            //string json= jsonHandler.SerializeMany(departmentsRes);
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine(json);
            
        }
    }
}
