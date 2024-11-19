using Generator;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ...");

            var taskFaker = new TaskFaker();
            var tasks = taskFaker.Generate(10);

            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }
    }
}
