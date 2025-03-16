using System;
using System.Collections.Generic;
using System.Linq;


namespace MovieModels
{
    public class Movie
    {
        private Guid Uuid;
        private Character Director;
        private List<Character> actors = new List<Character>();
        // -1 unrated; min 1 - max 10; example: rating 5 = 2.5 stars
        private int Rating { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Review { get; set; }

        public Movie()
        {
            this.Uuid = Guid.NewGuid();
            this.Name = "Undefined";
            this.Description = "Undefined";
            this.Rating = -1;
            this.Review = string.Empty;
            this.Director = new Character("null", DateTime.Today.Year);

        }

        public Movie(string name, string description)
        {
            this.Uuid = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Rating = -1;
            this.Review = string.Empty;
            this.Director = new Character("null null", DateTime.Today.Year);
        }

        public Movie(Guid uuid, string name, string description)
        {
            this.Uuid = uuid;
            this.Name = name;
            this.Description = description;
            this.Rating = -1;
            this.Review = string.Empty;
            this.Director = new Character("null null", DateTime.Today.Year);
        }

        public Movie(string fileInfo)
        {
            string[] data = fileInfo.Split(';');
            this.Uuid = Guid.Parse(data[0]);
            this.Name = data[1];
            this.Description = data[2];
            this.Rating = Int32.Parse(data[3]);
            this.Review = data[4];
            string[] directorData = data[5].Split('|');
            this.Director = new Character(directorData[0], DateTime.Parse(directorData[1]).Year);
            string[] actorNames = data[6].Split('|');
            string[] actorBirths = data[7].Split('|');
            for (int i = 0; i < actorNames.Length; i++)
            {
                this.actors.Add(new Character(actorNames[i], DateTime.Parse(directorData[1]).Year));
            }
        }

        public Guid GetUUID()
        {
            return this.Uuid;
        }

        public void AddRating(int rating)
        {
            if (rating < 0 || rating > 10)
            {
                throw new ArgumentOutOfRangeException("Rating must be between 0 and 10");
            }
            this.Rating = rating;
        }

        public void AddReview(string review)
        {
            this.Review = review;
        }

        public void AddDirector(Character director)
        {
            this.Director = director;
        }
        
        public void AddActor(Character actor)
        {
            this.actors.Add(actor);
        }

        public void RemoveActor(Character actor)
        {
            this.actors.Remove(actor);
        }

        public Character GetDirector()
        {
            return Director;
        }

        public List<Character> GetActors()
        {
            return actors;
        }

        public int GetRating()
        {
            return Rating;
        }

        public string MovieInfo()
        {
            return $"UUID: {this.Uuid}\nNume: {this.Name}\nDescriere: {this.Description}\nRating: {(this.Rating == -1 ? "Fara rating" : Convert.ToString((float)Rating / 2))}\nReview: {(this.Review == string.Empty ? "Fara review" : this.Review)}";
        }

        public string DatabaseInfo()
        {
            return $"{this.Uuid};{this.Name};{this.Description};{this.Rating};{this.Review};{this.Director.FullName}|{this.Director.birth.ToString()};{string.Join("|",this.actors.ToArray().Select(a => a.FullName))};{string.Join("|", this.actors.ToArray().Select(a => a.birth.ToString()))}";
        }
    }
}
