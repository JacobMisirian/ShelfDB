using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Exceptions;

namespace ShelfDB.Components
{
    public class Table
    {
        public string Name { get; private set; }
        public List<ColumnEntry> Columns { get; private set; }
        public List<RowEntry> Entries { get; private set; }

        public Dictionary<string, int> ColumnDictionary { get; private set; }

        public Table(string name, List<ColumnEntry> columns, List<RowEntry> entries)
        {
            Name = name;
            Columns = columns;
            Entries = entries;
            ColumnDictionary = new Dictionary<string, int>();
            for (int i = 0; i < columns.Count; i++)
                ColumnDictionary.Add(columns[i].Name, i);
        }

        public Table AddColumn(string name, int size)
        {
            var column = new ColumnEntry(name, size);
            Columns.Add(column);
            ColumnDictionary.Add(column.Name, Columns.Count - 1);
            return this;
        }

        public Table ChangeColumnSize(string name, int newSize)
        {
            if (!ColumnDictionary.ContainsKey(name))
                throw new ColumnNotFoundException(this, name);
            Columns[ColumnDictionary[name]].Size = newSize;
            return this;
        }

        public Table RemoveColumn(string name)
        {
            if (!ColumnDictionary.ContainsKey(name))
                throw new ColumnNotFoundException(this, name);
            Columns.Remove(Columns[ColumnDictionary[name]]);
            if (ColumnDictionary.ContainsKey(name))
                ColumnDictionary.Remove(name);
            return this;
        }

        public Table AddRow(params object[] values)
        {
            List<byte[]> entries = new List<byte[]>();
            for (int i = 0; i < Columns.Count; i++)
                entries.Add(padBytes(Columns[i], values[i]));
            Entries.Add(new RowEntry(Columns, ColumnDictionary, entries));
            return this;
        }

        private byte[] padBytes(ColumnEntry column, object o)
        {
            byte[] bytes = getBytes(o);
            byte[] finalBytes = new byte[column.Size];
            for (int i = finalBytes.Length - bytes.Length; i < finalBytes.Length; i++)
                finalBytes[i] = bytes[i - (finalBytes.Length - bytes.Length)];
            return finalBytes;
        }

        public List<RowEntry> SelectRows(string columnName, object value)
        {
            List <RowEntry> result = new List<RowEntry>();
            int columnIndex = ColumnDictionary[columnName];
            byte[] byteValue = padBytes(Columns[columnIndex], getBytes(value));
            foreach (var entry in Entries)
            {
                byte[] columnValue = entry.Values[columnIndex];
                if (ASCIIEncoding.ASCII.GetString(columnValue) == ASCIIEncoding.ASCII.GetString(byteValue) && columnValue.Length == byteValue.Length)
                    result.Add(entry);
            }
            return result;
        }

        private byte[] getBytes(object o)
        {
            if (o is bool)
                return BitConverter.GetBytes((bool)o);
            if (o is byte)
                return BitConverter.GetBytes((byte)o);
            if (o is byte[])
                return (byte[])o;
            if (o is char)
                return BitConverter.GetBytes((char)o);
            if (o is double)
                return BitConverter.GetBytes((double)o);
            if (o is float)
                return BitConverter.GetBytes((float)o);
            if (o is int)
                return BitConverter.GetBytes((int)o);
            if (o is long)
                return BitConverter.GetBytes((long)o);
            if (o is short)
                return BitConverter.GetBytes((short)o);
            if (o is string)
                return ASCIIEncoding.ASCII.GetBytes((string)o);
            if (o is uint)
                return BitConverter.GetBytes((uint)o);
            if (o is ulong)
                return BitConverter.GetBytes((ulong)o);
            if (o is ushort)
                return BitConverter.GetBytes((ushort)o);

            throw new UnhandledDataTypeException(o, this);
        }
    }
}