using ConsoleProgram.Generator;
using ConsoleProgram.Setup;
using Core.Models.Exp1;
using Core.Models.Exp3;
using Generator;
using HybridDataAccess.DataSerializator;
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
            SqlEmployeeRepository sqlEmployeeRepository = new SqlEmployeeRepository();
            /*
            Console.WriteLine("Creating tables ...");
            sqlEmployeeRepository.ExecuteCreationTable();
            Console.WriteLine("Generating data ...");
            var (menagers, developers) = generatorService.GenerateDataManagersAndDevelopers(5000);
            */
            Console.WriteLine("Reading data ...");
            string json = jsonHandler.SerializeMany<SoftwareDevelopersAggModel>
                (sqlEmployeeRepository.GetProgrammingLanguagesCountDevelopersAndAvgYearsExp());
            Console.WriteLine(json);
        }
    }
}
