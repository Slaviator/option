using CodingHelmet.Optional;

namespace Demo.Models
{
    public class Person
    {
        public string Name { get; }
        private int Age { get; }
        private Color FavoriteColor { get; }

        public Person(string name, int age, Color favoriteColor)
        {
            Name = name;
            Age = age;
            FavoriteColor = favoriteColor;
        }

        public Option<Car> TryGetCar() =>
            Age >= 18 
                ? (Option<Car>)new Car($"{Name}'s {FavoriteColor.Label.ToLower()} car", FavoriteColor) 
                : None.Value;
    }
}
