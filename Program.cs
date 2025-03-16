using System;
using CatalogueModel;
using MovieModels;
using FileManager;
using System.Collections.Generic;


namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Catalogue catalogue = new Catalogue();
            while (true)
            {
                Console.WriteLine("=== MENIU test CATALOG FILME ===");
                Console.WriteLine("1. Adauga film");
                Console.WriteLine("2. Sterge film");
                Console.WriteLine("3. Afiseaza catalog");
                Console.WriteLine("4. Cautare dupa nume");
                Console.WriteLine("5. Salveaza in fisier");
                Console.WriteLine("6. Incarca din fisier");
                Console.WriteLine("7. Exit");
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
                        SalveazaFisier(catalogue);
                        break;
                    case 6:
                        IncarcaDinFisier(catalogue);
                        break;
                    case 7:
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
            Console.Write("\nRating film:");
            int rating = Int32.Parse(Console.ReadLine());
            Console.Write("\nReview film:");
            string review = Console.ReadLine();
            Console.Write("\nNumele regizorului:");
            string directorName = Console.ReadLine();
            Console.Write("\nAnul nasterii regizorului:");
            int directorBirth = Int32.Parse(Console.ReadLine());
            Console.Write("\nNumele actorilor (separate prin virgula):");
            string[] actorNames = Console.ReadLine().Split(',');
            Console.Write("\nAnul nasterii actorilor (separate prin virgula):");
            string[] actorBirths = Console.ReadLine().Split(',');
            Movie m = new Movie(nume, desc);
            m.AddRating(rating);
            m.AddReview(review);
            m.AddDirector(new Character(directorName, directorBirth));
            for (int i = 0; i < actorNames.Length; i++)
            {
                m.AddActor(new Character(actorNames[i], Int32.Parse(actorBirths[i])));
            }
            catalogue.AddMovie(m);
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

        static void SalveazaFisier(Catalogue catalogue)
        {
            MovieManagerText fileManager = new MovieManagerText("movies.txt");
            foreach (Movie m in catalogue.GetMovies())
            {
                fileManager.AddMovie(m.DatabaseInfo());
            }

        }

        static void IncarcaDinFisier(Catalogue catalogue)
        {
            MovieManagerText fileManager = new MovieManagerText("movies.txt");
            List<string> lines = fileManager.GetContent();
            foreach (string line in lines)
            {
                if (catalogue.FindMovieByUUID(line.Split(';')[0]) == null)
                    catalogue.AddMovie(new Movie(line));
            }
            Console.ReadKey();
        }
    }
}
