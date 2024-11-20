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
            SqlEmployeeTasksRepository db = new();
            var setup = new Experiment1SetupService(db,100,1);
            var res = setup.RunGenerateData(10);

            string json = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }
    }
}
