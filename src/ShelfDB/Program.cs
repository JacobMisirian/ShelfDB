using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Components;

namespace ShelfDB
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseSerializer serializer = new DatabaseSerializer(args[0]);
            if (args[1] == "-tc")
            {
                Database database = Database.CreateFromName("MY DB!");
                var table1 = database.AddTable("table1");
                table1.AddColumn("Names", 200);
                table1.AddColumn("Dates", 200);
                table1.AddRow("Jacob", "8/26/99");
                table1.AddRow("Michael", "9/30/98");
                serializer.WriteDatabase(database);
            }
            else if (args[1] == "-tr")
            {
                Database database = serializer.ReadDatabase();
                var table1 = database.GetTable("table1");
                foreach (var result in table1.SelectRows("Names", "Jacob"))
                    Console.WriteLine(result.GetDataAsString("Dates"));
            }
        }
    }
}
