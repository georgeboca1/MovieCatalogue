using System;


namespace MovieCatalogue
{
    internal class Movie
    {
        private Guid Uuid;
        private Character Director;
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

        }

        public Movie(string name, string description)
        {
            this.Uuid = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Rating = -1;
            this.Review = string.Empty;
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

        public int GetRating()
        {
            return Rating;
        }

        public string MovieInfo()
        {
            return $"UUID: {this.Uuid}\nNume: {this.Name}\nDescriere: {this.Description}\nRating: {(this.Rating == -1 ? "Fara rating" : Convert.ToString(Rating / 2))}\nReview: {(this.Review == string.Empty ? "Fara review" : this.Review)}";
        }
    }
}
