using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieModels;

namespace FileManager
{
    public class MovieManagerText
    {
        public string fileName;

        public MovieManagerText(string fileName)
        {
            this.fileName = fileName;
            Stream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
            fileStream.Close();
        }

        public void AddMovie(string info)
        {
            using (StreamWriter stream = new StreamWriter(this.fileName, true))
            {
                stream.WriteLine(info);
            }
        }
        
        public void RemoveMovie(Guid uuid)
        {
            List<string> lines = File.ReadAllLines(this.fileName).ToList();
            lines.RemoveAll(line => line.Contains(uuid.ToString()));
            // Clean the file
            File.WriteAllText(this.fileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.fileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }

        public List<string> GetContent()
        {
            return File.ReadAllLines(this.fileName).ToList();
        }

    }
}
