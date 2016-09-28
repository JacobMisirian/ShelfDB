using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Components;

namespace ShelfDB.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public new string Message {  get { return string.Format("Could not find table {0} in database {1}!", TableName, Database.Name); } }
        public Database Database { get; private set; }
        public string TableName { get; private set; }

        public TableNotFoundException(Database database, string tableName)
        {
            Database = database;
            TableName = tableName;
        }
    }
}
