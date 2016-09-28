using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfDB.Components
{
    public class ColumnEntry
    {
        public string Name { get; private set; }
        public int Size { get; set; }

        public ColumnEntry(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
