using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlDataAccess;

namespace ConsoleProgram.Setup
{
    internal abstract class SetupService<T>
    {
        protected readonly int _mode;
        protected readonly string _name;
        protected readonly int _size;
      

        public SetupService(int mode,string repo,int size)
        {
            _mode = mode;
            _size= size;
            _name = repo;
        }
        
        public List<T> RunSetupData() 
        {
            RunCreateTables();
            RunPrepareData();
            return RunGenerateData();
        }
        public void RunCreateTables()
        {
            Console.WriteLine("/// TABLE CREATION ///");
            CreateTables();
            Console.WriteLine("/--/ Tables were created successfull");
            Console.WriteLine("\n\n");
        }
        public List<T> RunGenerateData()
        {
            Console.WriteLine("/// GENERATE DATA ///");
            var res = GenerateData();
            Console.WriteLine("/--/ Data were generated successfull!");
            return res;
        }

        public void RunPopulateData()
        {
            Console.WriteLine("/// POPULATE DATA ///");
            PopulateData();
            Console.WriteLine("-- Data were populated! successfull!");
        }
        public void RunPrepareData()
        {
            Console.WriteLine("/// PREPARE DATA ///");
            PrepareData();
            Console.WriteLine("-- Data were prepared! successfull!");
        }

        protected abstract void CreateTables();
        protected abstract void CreateIndexes();
        protected abstract void PrepareData();
        protected abstract List<T> GenerateData();
        protected abstract void PopulateData();
    }
}
