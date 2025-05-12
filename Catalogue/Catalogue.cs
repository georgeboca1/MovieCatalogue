using System;
using MovieModels;
using System.Collections.Generic;


namespace CatalogueModel
{
    public class Catalogue
    {
        // This class contains a list of all available movies in the application
        public List<Movie> Movies;

        public Catalogue()
        {
            Movies = new List<Movie>();
        }

        public void AddMovie(Movie m)
        {
            this.Movies.Add(m);
        }

        public void AddMovies(List<Movie> m)
        {
            foreach(Movie mov in m)
            {
                this.Movies.Add(mov);
            }
        }

        public void RemoveMovie(Movie m)
        {
            Movies.Remove(m);
        }
        public int RemoveMovieByUUID(Guid uuid)
        {
            Movie m = Movies.Find(mov => mov.GetUUID() == uuid);
            if (m == null) return 0;
            Movies.Remove(m);
            return 1;
        }

        public Guid GetUUIDByName(string name)
        {
            Movie m = Movies.Find(mov => mov.Name == name);
            return m != null ? m.GetUUID() : Guid.Empty;
        }

        public Movie FindMovieByName(string name)
        {
            Movie m = Movies.Find(mov => mov.Name == name);
            return m;
        }

        public Movie FindMovieByUUID(string uuid)
        {
            Movie m = Movies.Find(mov => mov.GetUUID() == Guid.Parse(uuid));
            return m;
        }

        public List<Movie> FindMoviesByName(string name)
        {
            List<Movie> m = Movies.FindAll(mov => mov.Name == name);
            return m;
        }

        public List<Movie> GetMovies()
        {
            return Movies;
        }

        public void ClearCatalogue()
        {
            Movies.Clear();
        }
    }
}
