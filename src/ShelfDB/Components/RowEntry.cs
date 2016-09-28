using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Exceptions;

namespace ShelfDB.Components
{
    public class RowEntry
    {
        public List<ColumnEntry> Columns { get; private set; }
        public Dictionary<string, int> ColumnDictionary { get; private set; }
        public List<byte[]> Values { get; private set; }

        public RowEntry(List<ColumnEntry> columns, Dictionary<string, int> columnDictionary, List<byte[]> values)
        {
            Columns = columns;
            ColumnDictionary = columnDictionary;
            Values = values;
        }

        public bool GetDataAsBool(string columnName)
        {
            return BitConverter.ToBoolean(GetData(columnName, sizeof(bool)), 0);
        }
        public byte GetDataAsByte(string columnName)
        {
            return GetData(columnName, sizeof(byte))[0];
        }
        public char GetDataAschar(string columnName)
        {
            return BitConverter.ToChar(GetData(columnName, sizeof(char)), 0);
        }
        public double GetDataAsFloat(string columnName)
        {
            return BitConverter.ToDouble(GetData(columnName, sizeof(double)), 0);
        }
        public int GetDataAsInt(string columnName)
        {
            return BitConverter.ToInt32(GetData(columnName, sizeof(int)), 0);
        }
        public long GetDataAsLong(string columnName)
        {
            return BitConverter.ToInt64(GetData(columnName, sizeof(long)), 0);
        }
        public short GetDataAsShort(string columnName)
        {
            return BitConverter.ToInt16(GetData(columnName, sizeof(short)), 0);
        }
        public string GetDataAsString(string columnName)
        {
            return ASCIIEncoding.ASCII.GetString(GetData(columnName));
        }
        public ushort GetDataAsUshort(string columnName)
        {
            return BitConverter.ToUInt16(GetData(columnName, sizeof(ushort)), 0);
        }
        public uint GetDataAsUint(string columnName)
        {
            return BitConverter.ToUInt32(GetData(columnName, sizeof(uint)), 0);
        }
        public ulong GetDataAsUlong(string columnName)
        {
            return BitConverter.ToUInt64(GetData(columnName, sizeof(ulong)), 0);
        }
        public byte[] GetData(string columnName, int size = -1)
        {
            int columnIndex = ColumnDictionary[columnName];
            if (size != -1)
                return Values[columnIndex].ToList().GetRange(Values[columnIndex].Length - size - 1, size).ToArray();
            return Values[columnIndex];
        }
    }
}
