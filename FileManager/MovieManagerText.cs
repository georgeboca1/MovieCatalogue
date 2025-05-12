using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogueModel;
using MovieModels;

namespace FileManager
{
    public class MovieManagerText
    {
        public string movieFileName;
        public string actorsFileName;
        public string directorFileName;


        public MovieManagerText(string fileName, string actorsFileName, string directorFileName)
        {
            this.movieFileName = fileName;
            this.actorsFileName = actorsFileName;
            this.directorFileName = directorFileName;
            Stream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
            fileStream.Close();
        }

        public void AddMovie(string info)
        {
            using (StreamWriter stream = new StreamWriter(this.movieFileName, true))
            {
                stream.WriteLine(info);
            }
        }

        public void AddActor(string info)
        {
            using (StreamWriter stream = new StreamWriter(this.actorsFileName, true))
            {
                stream.WriteLine(info);
            }
        }

        public void AddDirector(string info)
        {
            using (StreamWriter stream = new StreamWriter(this.directorFileName, true))
            {
                stream.WriteLine(info);
            }
        }
        
        public void RemoveMovie(Guid uuid)
        {
            List<string> lines = File.ReadAllLines(this.movieFileName).ToList();
            lines.RemoveAll(line => line.Contains(uuid.ToString()));
            // Clean the file
            File.WriteAllText(this.movieFileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.movieFileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }

        public void RemoveActor(string actor)
        {
            List<string> lines = File.ReadAllLines(this.actorsFileName).ToList();
            lines.RemoveAll(line => line.Contains(actor));
            // Clean the file
            File.WriteAllText(this.actorsFileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.actorsFileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }

        public void RemoveDirector(string director)
        {
            List<string> lines = File.ReadAllLines(this.directorFileName).ToList();
            lines.RemoveAll(line => line.Contains(director));
            // Clean the file
            File.WriteAllText(this.directorFileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.directorFileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }

        public List<Character> GetActors()
        {
            List<Character> actors = new List<Character>();
            List<string> strings = File.ReadAllLines(this.actorsFileName).ToList();
            for (int i = 0; i < strings.Count; i++)
            {
                string[] s = strings[i].Split(';');
                actors.Add(new Character(Guid.Parse(s[0]), s[1], Int32.Parse(s[2])));
            }
            return actors;

        }
        public List<Character> GetDirectors()
        {
            List<Character> directors = new List<Character>();
            List<string> strings = File.ReadAllLines(this.directorFileName).ToList();
            for (int i = 0; i < strings.Count; i++)
            {
                string[] s = strings[i].Split(';');
                directors.Add(new Character(Guid.Parse(s[0]), s[1],Int32.Parse(s[2])));
            }
            return directors;
        }

        public void GetMovies(ref Catalogue catalogue)
        {
            List<Movie> movies = new List<Movie>();
            List<string> strings = File.ReadAllLines(this.movieFileName).ToList();
            for (int i = 0; i < strings.Count; i++)
            {
                Movie m = new Movie(strings[i]);
                string s = strings[i].Split(';')[5];
                if (string.IsNullOrEmpty(s))
                {
                    m.AddDirector(new Character("null null", DateTime.Today.Year));
                }
                else
                {
                    m.AddDirector(GetDirector(Guid.Parse(s)));
                }
                s = strings[i].Split(';')[6];
                if (!string.IsNullOrEmpty(s))
                {
                    foreach (string actor in s.Split('|'))
                    {
                        m.AddActor(GetActor(Guid.Parse(actor)));
                    }
                }
                movies.Add(m);
            }
            catalogue.AddMovies(movies);
        }

        public Movie GetMovieByName(string name)
        {
            List<string> strings = File.ReadAllLines(this.movieFileName).ToList();
            for (int i = 0; i < strings.Count; i++)
            {
                string[] s = strings[i].Split(';');
                if (s[1] == name)
                {
                    Movie m = new Movie(strings[i]);
                    return m;
                }
            }
            return null;
        }

        static Character GetDirector(Guid uuid)
        {
            for (int i = 0; i < File.ReadAllLines("directors.txt").Length; i++)
            {
                string[] s = File.ReadAllLines("directors.txt")[i].Split(';');
                if (Guid.Parse(s[0]) == uuid)
                {
                    return new Character(s[1], Int32.Parse(s[2]));
                }
            }
            return null;
        }

        static Character GetActor(Guid uuid)
        {
            for (int i = 0; i < File.ReadAllLines("actors.txt").Length; i++)
            {
                string[] s = File.ReadAllLines("actors.txt")[i].Split(';');
                if (Guid.Parse(s[0]) == uuid)
                {
                    return new Character(s[1], Int32.Parse(s[2]));
                }
            }
            return null;
        }

        public void UpdateMovie(Movie movie)
        {
            List<string> lines = File.ReadAllLines(this.movieFileName).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains(movie.GetUUID().ToString()))
                {
                    lines[i] = movie.DatabaseInfo();
                    break;
                }
            }
            // Clean the file
            File.WriteAllText(this.movieFileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.movieFileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }

        public void DeleteMovie(Movie movie)
        {
            List<string> lines = File.ReadAllLines(this.movieFileName).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains(movie.GetUUID().ToString()))
                {
                    lines.RemoveAt(i);
                    break;
                }
            }
            // Clean the file
            File.WriteAllText(this.movieFileName, string.Empty);
            // Write the new lines
            using (StreamWriter stream = new StreamWriter(this.movieFileName))
            {
                foreach (string line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }
    }
}
