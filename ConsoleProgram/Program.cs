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
            var manager= new ManagerModel()
            {
                Id=1,
                FirstName="A",
                LastName="B",
                BirthDay=DateTime.Now,
                Email="C",
                Phone="D",
                RealisedProject=2,
                Method="E"
            };

            var sql = new SqlEmployeeRepository();
            //sql.InsertManager(manager);
            sql.ExecuteCreationTable();
        }
    }
}
