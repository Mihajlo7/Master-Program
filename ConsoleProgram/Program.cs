using ConsoleProgram.Setup;
using Core.Models.Exp1;
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
            Console.WriteLine("Start ...");
            var setup = new Experiment1SetupService("hybrid",SetSizeInterface.SMALL_SET,1);
            //setup.RunSetupDatabase();
            var res= setup.RunSetupData();
            //setup.RunPopulateData();
            /*
            SqlEmployeeTasksRepository sql = new();
            JsonHandler handler = new JsonHandler();
            var res =
                sql.GetEmployeesWithCountTasksHavingAndOrder(12);
                //sql.GetTaskWithEmployeesById(10);
            var json = handler.SerializeMany<EmployeeWithCountTasksModel>(res);
            Console.WriteLine(json);
            */

            MongoEmployeeTasksRepository mongo = new();
            JsonHandler handler = new JsonHandler();
            var result = mongo.UpdatePhoneById("Novi broj",23);
            //var json = handler.SerializeMany<EmployeeWithCountTasksModel>(result);
            Console.WriteLine(result);
            //mongo.DeleteAllTasks();
        }
    }
}
