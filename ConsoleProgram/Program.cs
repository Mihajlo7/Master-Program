using ConsoleProgram.Setup;
using Generator;
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
            //var setup = new Experiment1SetupService("sql",SetSizeInterface.SMALL_SET,1);
            //var res= setup.RunSetupData();
            //setup.RunPopulateData();
            SqlEmployeeTasksRepository sql = new();
            var res = sql.GetTaskWithEmployeesById(38);
            string json = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }
    }
}
