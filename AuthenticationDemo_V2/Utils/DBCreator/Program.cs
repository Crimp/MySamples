using BusinessObjectsLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBCreator {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Create data base or update");
            try {
                string connectionString = ConfigurationManager.ConnectionStrings["AzureApplicationServices"].ConnectionString;
                DBUpdater updater = new DBUpdater(connectionString, new Assembly[] { typeof(Contact).Assembly });
                updater.CreateDB();
            }
            catch(Exception e) {
                Console.WriteLine("Exception:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Done");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
