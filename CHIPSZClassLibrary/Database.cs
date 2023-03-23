using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CHIPSZClassLibrary
{
    public class Database
    {
        private Dictionary<string, string> records;
        private string filename;

        public Database(string filename)
        {
            this.filename = filename;
            records = new Dictionary<string, string>();

            if(System.IO.File.Exists(filename))
            {
                ReadAll();
            }
            else
            {
                System.IO.File.Create(filename);

            }
        }

        public Database()
        {
        }

        public void ReadAll()
        {

            foreach (string line in System.IO.File.ReadAllLines(filename))
            {
                if (!string.IsNullOrEmpty(line) && line.Contains('='))
                {
                    int idx = line.IndexOf('=');    
                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    records.Add(key, value);
                }
            }
        }

        public void Add(string field, string value)
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(filename);
            streamWriter.WriteLine($"{field}={value}");
            streamWriter.Close();
        }

    }
}

