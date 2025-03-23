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
            MovieManagerText fileManager = new MovieManagerText("movies.txt", "actors.txt", "directors.txt");
            Movie selectedMovie = null;
            while (true)
            {
                int consoleWidth = Console.WindowWidth - 10;
                int midPoint = consoleWidth - 40; // Adjust 40 depending on how much space you want on the right

                Console.WriteLine($"=== MENIU CATALOG ===".PadRight(midPoint) + $"{(selectedMovie != null ? "Film selectat:" + selectedMovie.GetUUID() : "")}");
                Console.WriteLine($"1. Adauga film".PadRight(midPoint) + $"{selectedMovie?.Name ?? ""}");
                Console.WriteLine($"2. Creeaza caracter");
                Console.WriteLine($"3. Selecteaza film".PadRight(midPoint) + $"{(selectedMovie != null ? "=== MENIU FILM ===" : "")}");
                Console.WriteLine($"4. Sterge film".PadRight(midPoint) + $"{(selectedMovie != null ? "A. Adauga rating" : "")}");
                Console.WriteLine($"5. Afiseaza catalog".PadRight(midPoint) + $"{(selectedMovie != null ? "B. Adauga review" : "")}");
                Console.WriteLine($"6. Cautare dupa nume".PadRight(midPoint) + $"{(selectedMovie != null ? "C. Afiseaza informatii film" : "")}");
                Console.WriteLine($"7. Salveaza in fisier");
                Console.WriteLine($"8. Incarca din fisier");
                Console.WriteLine("9. Exit\n\n");
                Console.Write(">>".PadLeft(midPoint - 15));
                string i = Console.ReadLine();

                switch (i)
                {
                    case "1":
                        AdaugaFilm(catalogue, ref fileManager);
                        break;
                    case "2":
                        CreeazaCaracter(catalogue, ref fileManager);
                        break;
                    case "3":
                        selectedMovie = SelecteazaFilm(catalogue);
                        break;
                    case "4":
                        StergeFilm(catalogue);
                        break;
                    case "5":
                        AfiseazaFilme(catalogue);
                        break;
                    case "6":
                        CautaFilm(catalogue);
                        break;
                    case "7":
                        SalveazaFisier(catalogue, ref fileManager);
                        break;
                    case "8":
                        IncarcaDinFisier(catalogue, ref fileManager);
                        break;
                    case "9":
                        Environment.Exit(0);
                        break;
                    case "A":
                        if (selectedMovie == null) break;
                        Console.WriteLine("Adauga rating filmului selectat");
                        Console.Write("Rating: ");
                        int rating = Int32.Parse(Console.ReadLine());
                        selectedMovie.AddRating(rating);
                        break;
                    case "B":
                        if (selectedMovie == null) break;
                        Console.WriteLine("Adauga review filmului selectat");
                        Console.Write("Review: ");
                        string review = Console.ReadLine();
                        selectedMovie.AddReview(review);
                        break;
                    case "C":
                        if (selectedMovie == null) break;
                        Console.WriteLine(selectedMovie.MovieInfo());
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Optiune invalida");
                        break;
                }
                Console.Clear();
            }
        }

        static void AdaugaFilm(Catalogue catalogue, ref MovieManagerText fileManager)
        {
            Console.Write("\nNumele filmului care vrei sa il adaugi:");
            string nume = Console.ReadLine();

            Console.Write("\nDescrierea filmului:");
            string desc = Console.ReadLine();

            Console.Write("\nGenul filmului:");
            GenreType genre = (GenreType)Enum.Parse(typeof(GenreType), Console.ReadLine());

            Console.Write("\nRating film:");
            int rating = Int32.Parse(Console.ReadLine());

            Console.Write("\nReview film:");
            string review = Console.ReadLine();

            Console.WriteLine("Regizori deja creati:");
            List<Character> directori = fileManager.GetDirectors();
            for (int i = 0; i < directori.Count; i++)
            {
                Console.WriteLine($"{i}. {directori[i].FullName}");
            }
            Console.Write("Selecteaza regizorul filmului (numarul din lista):");
            int directorIndex = Int32.Parse(Console.ReadLine());
            
            Console.WriteLine("\nActorii filmului:");
            List<Character> actori = fileManager.GetActors();
            for (int i = 0; i < actori.Count; i++)
            {
                Console.WriteLine($"{i}. {actori[i].FullName}");
            }
            Console.Write("Actorii filmului (numerele din lista, separate prin spatiu):");
            string[] actorIndexes = Console.ReadLine().Split(' ');

            Movie m = new Movie(nume, desc);
            m.AddRating(rating);
            m.AddReview(review);
            m.AddDirector(directori[directorIndex]);
            m.AddGenre(genre);
            foreach (string actorIndex in actorIndexes)
            {
                m.AddActor(actori[Int32.Parse(actorIndex)]);
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

        static void SalveazaFisier(Catalogue catalogue, ref MovieManagerText fileManager)
        {

            foreach (Movie m in catalogue.GetMovies())
            {
                fileManager.AddMovie(m.DatabaseInfo());
            }

        }

        static void IncarcaDinFisier(Catalogue catalogue, ref MovieManagerText fileManager)
        {
            fileManager.GetMovies(ref catalogue);
        }

        static Movie SelecteazaFilm(Catalogue catalogue)
        {
            if (catalogue.GetMovies().Count == 0)
            {
                Console.WriteLine("Adauga filme in catalog inainte de a selecta un film");
                Console.ReadKey();
                return null;
            }
            else
            {
                Console.WriteLine("Filme in lista:");
                foreach (Movie m in catalogue.GetMovies())
                {
                    Console.WriteLine(m.MovieInfo());
                }
            }
            int opt = 9999;
            do
            {
                Console.Write("\nNumarul fimului care doresti sa-l selectezi\n>>");
                opt = Int32.Parse(Console.ReadLine());
            }
            while (opt >= catalogue.GetMovies().Count);
            return catalogue.GetMovies()[opt];
        }
        
        static void CreeazaCaracter(Catalogue catalogue, ref MovieManagerText FileManager)
        {
            Console.Write("Director/Actor:");
            string type = Console.ReadLine();
            Console.Write("\nNumele caracterului:");
            string nume = Console.ReadLine();
            Console.Write("\nAnul nasterii caracterului:");
            int birth = Int32.Parse(Console.ReadLine());
            Character c = new Character(nume, birth);
            if (type == "Director")
            {
                FileManager.AddDirector(c.toFile());
            }
            else if (type == "Actor")
            {
                FileManager.AddActor(c.toFile());
            }
            else
            {
                Console.WriteLine("Invalid type.");
            }
        }
    }
}
