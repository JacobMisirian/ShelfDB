using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using ShelfDB.Components;

namespace ShelfDB
{
    public class DatabaseSerializer
    {
        private BinaryReader reader;
        private BinaryWriter writer;

        public DatabaseSerializer(string databaseFilePath)
        {
            var stream = File.Open(databaseFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public void WriteDatabase(Database database)
        {
            writer.BaseStream.Position = 0;
            writer.Write(database.Name);
            foreach (var table in database.Tables.Values)
                writeTable(table);
            writer.Flush();
        }

        private void writeTable(Table table)
        {
            writer.Write(table.Name);
            writer.Write(table.Columns.Count);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                writer.Write(table.Columns[i].Name);
                writer.Write(table.Columns[i].Size);
            }
            writer.Write(table.Entries.Count);
            for (int i = 0; i < table.Entries.Count; i++)
                writeEntry(table.Entries[i]);
        }

        private void writeEntry(RowEntry entry)
        {
            foreach (var arr in entry.Values)
                writer.Write(arr);
        }

        public Database ReadDatabase()
        {
            string name;
            Dictionary<string, Table> tables = new Dictionary<string, Table>();

            reader.BaseStream.Position = 0;
            name = reader.ReadString();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var table = readTable();
                tables.Add(table.Name, table);
            }

            return new Database(name, tables);
        }

        private Table readTable()
        {
            string name;
            List<ColumnEntry> columns = new List<ColumnEntry>();
            Dictionary<string, int> columnDictionary = new Dictionary<string, int>();
            List<RowEntry> entries = new List<RowEntry>();
            int columnCount, entryCount;
            
            name = reader.ReadString();
            columnCount = reader.ReadInt32();
            for (int i = 0; i < columnCount; i++)
            {
                var column = readColumnEntry();
                columns.Add(column);
                columnDictionary.Add(column.Name, i);
            }

            entryCount = reader.ReadInt32();
            for (int i = 0; i < entryCount; i++)
                entries.Add(readRowEntry(columns, columnDictionary));

            return new Table(name, columns, entries);
        }

        private ColumnEntry readColumnEntry()
        {
            string name = reader.ReadString();
            int size = reader.ReadInt32();

            return new ColumnEntry(name, size);
        }

        private RowEntry readRowEntry(List<ColumnEntry> columns, Dictionary<string, int> columnDictionary)
        {
            List<byte[]> values = new List<byte[]>();
            for (int i = 0; i < columns.Count; i++)
                values.Add(reader.ReadBytes(columns[i].Size));
            return new RowEntry(columns, columnDictionary, values);
        }
    }
}
