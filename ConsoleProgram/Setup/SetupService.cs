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
      

        public SetupService(int mode,object repo,int size)
        {
            _mode = mode;
            _size= size;
            if(repo is SqlRepository)
            {
                _name = "sql";
            }
            else
            {
                _name = "mongo";
            }
        }
        public void RunSetup()
        {
            Console.WriteLine("///////////////////");
            Console.WriteLine("//// RUN SETUP //// ");
            Console.WriteLine("\n\n");
            RunCreateTables();
            if(_mode==2)
            {
                Console.WriteLine("/// INDEX CREATION ///");
                CreateIndexes();
                Console.WriteLine("-- Indexes were created succefull");
                Console.WriteLine("\n\n");
            }
            //RunGenerateData();
            RunPopulateData();
            
        }
        public void RunCreateTables()
        {
            Console.WriteLine("/// TABLE CREATION ///");
            CreateTables();
            Console.WriteLine("/--/ Tables were created successfull");
            Console.WriteLine("\n\n");
        }
        public List<T> RunGenerateData(int links)
        {
            Console.WriteLine("/// GENERATE DATA ///");
            var res = GenerateData(links);
            Console.WriteLine("/--/ Data were generated successfull!");
            return res;
        }

        public void RunPopulateData()
        {
            Console.WriteLine("/// POPULATE DATA ///");
            PopulateData();
            Console.WriteLine("-- Data were populated! successfull!");
        }

        protected abstract void CreateTables();
        protected abstract void CreateIndexes();
        protected abstract List<T> GenerateData(int links);
        protected abstract void PopulateData();
    }
}
