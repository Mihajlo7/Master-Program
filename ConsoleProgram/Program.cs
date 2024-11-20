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
            var setup = new Experiment1SetupService("sql",100,1);
            //var res = setup.RunGenerateData(10);
            //setup.RunCreateTables();
            setup.RunPrepareData();
            setup.RunPopulateData(10);
            //string json = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
            //Console.WriteLine(json);
        }
    }
}
