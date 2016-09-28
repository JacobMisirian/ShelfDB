using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Components;

namespace ShelfDB.Exceptions
{
    public class ColumnNotFoundException : Exception
    {
        public new string Message {  get { return string.Format("Could not find column {0} in table {1}!", ColumnName, Table.Name); } }
        public Table Table { get; private set; }
        public string ColumnName { get; private set; }

        public ColumnNotFoundException(Table table, string columnName)
        {
            Table = table;
            ColumnName = columnName;
        }
    }
}
