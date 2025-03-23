using System;


namespace MovieModels
{
    public class Character
    {
        private Guid UUID;
        public DateTime birth;

        private string FirstName;
        private string LastName;
        public string FullName { get { return $"{FirstName} {LastName}"; } set { FirstName = value.Split()[0]; LastName = value.Split()[1]; } }

        public Character(string name, int birth_year)
        {
            UUID = Guid.NewGuid();
            FullName = name;
            birth = new DateTime(birth_year,1,1);
        }

        public Character(Guid uuid, string name, int birth_year)
        {
            UUID = uuid;
            FullName = name;
            birth = new DateTime(birth_year, 1, 1);
        }

        public string CharacterInfo()
        {
            return $"Nume: {FullName}\nVarsta:{birth.Year}";
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public Guid GetUUID()
        {
            return UUID;
        }

        public string toFile()
        {
            return $"{UUID};{FullName};{birth.Year}";
        }
    }
}
