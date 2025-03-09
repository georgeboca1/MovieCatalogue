using System;
using System.Collections.Generic;


namespace MovieCatalogue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Catalogue catalogue = new Catalogue();
            while (true)
            {
                Console.WriteLine("=== MENIU CATALOG FILME ===");
                Console.WriteLine("1. Adauga film");
                Console.WriteLine("2. Sterge film");
                Console.WriteLine("3. Afiseaza catalog");
                Console.WriteLine("4. Cautare dupa nume");
                Console.WriteLine("5. Exit");
                Console.Write(">>");
                Int32.TryParse(Console.ReadLine(), out int i);

                switch (i)
                {
                    case 1:
                        AdaugaFilm(catalogue);
                        break;
                    case 2:
                        StergeFilm(catalogue);
                        break;
                    case 3:
                        AfiseazaFilme(catalogue);
                        break;
                    case 4:
                        CautaFilm(catalogue);
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }
                Console.Clear();
            }
        }

        static void AdaugaFilm(Catalogue catalogue)
        {
            Console.Write("\nNumele filmului care vrei sa il adaugi:");
            string nume = Console.ReadLine();
            Console.Write("\nDescrierea filmului:");
            string desc = Console.ReadLine();
            catalogue.AddMovie(new Movie(nume, desc));
        }

        static void StergeFilm(Catalogue catalogue)
        {
            Console.Write("Numele filmului care vrei sa il stergi: ");
            string _nume = Console.ReadLine();
            if (catalogue.RemoveMovieByUUID(catalogue.GetUUIDByName(_nume)) == 1)
                Console.WriteLine("Filmul a fost sters.");
            else
                Console.WriteLine("Filmul nu a fost gasit.");
            Console.ReadKey();
        }

        static void AfiseazaFilme(Catalogue catalogue)
        {
            Console.WriteLine("Filme in lista:");
            if (catalogue.GetMovies().Count == 0) Console.WriteLine("Momentan nu ai niciun film in catalog.");
            else
            {
                foreach (Movie m in catalogue.GetMovies())
                {
                    Console.WriteLine(m.MovieInfo());
                }
            }
            Console.ReadKey();
        }

        static void CautaFilm(Catalogue catalogue)
        {
            Console.Write("Numele filmului cautat: ");
            string MovieName = Console.ReadLine() ?? string.Empty;

            List<Movie> m = catalogue.FindMoviesByName(MovieName);
            if (m.Count == 0) { Console.WriteLine("Filmul introdus nu exista in catalog."); Console.ReadKey(); return; }
            foreach (Movie movie in m) Console.WriteLine(movie.MovieInfo());

            Console.ReadKey();
        }

    }
}
