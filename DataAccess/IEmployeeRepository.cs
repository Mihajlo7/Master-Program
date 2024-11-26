using Core.Models.Exp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IEmployeeRepository
    {
        public abstract void ExecuteCreationTable();
        public abstract void ExecuteCreationAdditional();
        public void InsertManager(ManagerModel manager);
        public void InsertSoftwareDeveloper(SoftwareDeveloperModel softwareDeveloper);
        public void InsertManyManager(List<ManagerModel> managers);
        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers);
        public void InsertBulkManager(List<ManagerModel> managers);
        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers);

        //Vrati sve zaposlene
        public List<EmployeeModel3> GetAllEmployees();
        //Vrati sve menadzere
        public List<ManagerModel> GetAllManagers();
        //Vrati sve programere
        public List<SoftwareDeveloperModel> GetAllSoftwareDevelopers();
        //Vrati zaposlenog po Id
        public EmployeeModel3 GetEmployeeById(long id);
        //Vrati menadzere koji su mladji od 30 g i rade Agil i sortiraj po datumu 
        public List<ManagerModel> GetManagersYoungerThan30AndAgileMethodSorted();
        // Vrati programere koji rade remote i koji rade na visual studio
        public List<SoftwareDeveloperModel> GetSoftwareDevelopersRemoteAndUseVisualStudio();
        // Vrate programere koji su stariji od 25g i da su mediori i da rade u Javi i sortiraj po godinama iskustva
        public List<SoftwareDeveloperModel> GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC();
        // Grupisi menadzere po metodi i pikazi koliko ih ima
        public List<ManagerAggModel> GetAllMethodsWithCountManagers();
        // Grupisi programere po programskom jeyiku i prikazi samo gde ih ima vise od 10,prikazi i avg dodina iskustva po programskom jeziku
        public List<SoftwareDevelopersAggModel> GetProgrammingLanguagesCountDevelopersAndAvgYearsExp();

        // Update phone po Id
        // Update method po Id
        // Update fullstack po Id
        // Update method na Lean gde su izmedju 40 i 50 g i rade u It ili Logistici
        // Update fullstack true gde su godina experience veca 10 i gde titula sadryi engineer
        // Update title dodaj na pocetak title Principe ako je fullstack i ako je senior i radi duye od 20 godina
    }
}
