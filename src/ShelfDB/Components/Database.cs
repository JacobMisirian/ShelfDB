using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Exceptions;

namespace ShelfDB.Components
{
    public class Database
    {
        public static Database CreateFromName(string name)
        {
            return new Database(name, new Dictionary<string, Table>());
        }

        public string Name { get; private set; }
        public Dictionary<string, Table> Tables { get; private set; }

        public Database(string name, Dictionary<string, Table> tables)
        {
            Name = name;
            Tables = tables;
        }

        public Table AddTable(string name)
        {
            var ret = new Table(name, new List<ColumnEntry>(), new List<RowEntry>());
            Tables.Add(name, ret);
            return ret;
        }
        public Table AddTable(string name, List<ColumnEntry> columns)
        {
            var ret = new Table(name, columns, new List<RowEntry>());
            Tables.Add(name, ret);
            return ret;
        }
        public Table AddTable(string name, List<ColumnEntry> columns, List<RowEntry> entries)
        {
            var ret = new Table(name, columns, entries);
            Tables.Add(name, ret);
            return ret;
        }

        public void DeleteTable(string name)
        {
            if (!Tables.ContainsKey(name))
                throw new TableNotFoundException(this, name);
            Tables.Remove(name);
        }

        public Table GetTable(string name)
        {
            if (!Tables.ContainsKey(name))
                throw new TableNotFoundException(this, name);
            return Tables[name];
        }
    }
}
