using System;


namespace MovieModels
{
    public class Character
    {
        public DateTime birth;

        private string FirstName;
        private string LastName;
        public string FullName { get { return $"{FirstName} {LastName}"; } set { FirstName = value.Split()[0]; LastName = value.Split()[1]; } }

        public Character(string name, int birth_year)
        {
            FullName = name;
            birth = new DateTime(birth_year,1,1);
        }

        public string CharacterInfo()
        {
            return $"Nume: {FullName}\nVarsta:{birth.Year}";
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
