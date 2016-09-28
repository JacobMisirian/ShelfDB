using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Components;

namespace ShelfDB.Exceptions
{
    public class UnhandledDataTypeException : Exception
    {
        public new string Message { get { return string.Format("Unhandled data type {0} in table {1}!", Object.GetType(), Table.Name); } }
        public object Object { get; private set; }
        public Table Table { get; private set; }

        public UnhandledDataTypeException(object obj, Table table)
        {
            Object = obj;
            Table = table;
        }
    }
}
