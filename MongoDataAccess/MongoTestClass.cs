using MongoDB.Driver;
namespace MongoDataAccess
{
    public class MongoTestClass
    {
        string mongoUrl =
             "mongodb+srv://pavmihajlo:Master@cluster0.a9sgb.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        MongoClient mongoClient;

        public MongoTestClass() 
        {
            mongoClient = new MongoClient(mongoUrl);
        }

        public void GetDatabases()
        {
            Console.WriteLine("The list of database on server is ");

            var dbList=mongoClient.ListDatabaseNames().ToList();
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
        }
        
    }
}
