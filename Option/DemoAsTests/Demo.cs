using System.Collections.Generic;
using System.Linq;
using CodingHelmet.Optional;
using CodingHelmet.Optional.Extensions;
using Demo.Models;
using Xunit;
using Xunit.Abstractions;

namespace Demo.Tests
{
    public class Demo
    {
        public ITestOutputHelper Console { get; }

        public Demo(ITestOutputHelper testOutputHelper)
        {
            Console = testOutputHelper;
        }

        [Fact]
        public void ImplicitOperatorsDemo()
        {
            Console.WriteLine("*** Implicit conversion demo:");
            Option<Car> converted = new Car("red car", Color.Red);
            Some<Car> convertedSome = new Car("blue car", Color.Blue);
            Car extracted = convertedSome;

            Console.WriteLine(converted.ToString());
            Console.WriteLine(convertedSome.ToString());
            Console.WriteLine(extracted.ToString());
        }

        [Fact]
        public void MappingDemo()
        {
            Console.WriteLine("*** Mapping demo:");

            Person child = new Person("Jill", 12, Color.Red);
            Person grownUp = new Person("Joe", 46, Color.Blue);

            Option<Car> none = child.TryGetCar(); // None
            Option<Car> some = grownUp.TryGetCar();

            Console.WriteLine($"{none}, {some}");

            Option<Person> noPerson = None.Value;
            Option<Person> someChild = child;
            Option<Person> someGrownUp = grownUp;

            Option<Car> noCar = noPerson.MapOptional(person => person.TryGetCar());
            Option<Car> noChildCar = someChild.MapOptional(person => person.TryGetCar());
            Option<Car> someGrownUpCar = someGrownUp.MapOptional(person => person.TryGetCar());

            Console.WriteLine($"{noCar}, {noChildCar}, {someGrownUpCar}");
        }

        [Fact]
        public void ObjectNoneIfNullDemo()
        {
            Console.WriteLine("*** Object.NoneIfNull() demo:");
            Color color = Color.Red;
            Option<Color> maybeColor1 = color.NoneIfNull(); // Some(Red)
            Console.WriteLine($"{color} -> {maybeColor1}");

            color = null;
            Option<Color> maybeColor2 = color.NoneIfNull(); // None
            Console.WriteLine($"{color} -> {maybeColor2}");
        }

        [Fact]
        public void ObjectWhenDemo()
        {
            Console.WriteLine("*** Object.When() demo:");
            Color red = Color.Red;
            Option<Color> beautiful = red.When(red == Color.Red); // Some(Red)
            Console.WriteLine($"{red} -> {beautiful}");

            Color blue = Color.Blue;
            Option<Color> ugly = blue.When(c => c == Color.Red); // None
            Console.WriteLine($"{blue} -> {ugly}");
        }

        [Fact]
        public void OptionOfTypeDemo()
        {
            Console.WriteLine("*** Option.OfType() demo:");
            Option<Car> someCar = new Car("car", Color.Red);
            Option<Car> noCar = None.Value;

            Console.WriteLine(someCar.OfType<Vehicle>().ToString()); // Some
            Console.WriteLine(noCar.OfType<Vehicle>().ToString()); // None
            Console.WriteLine(someCar.OfType<Truck>().ToString()); // None
        }

        [Fact]
        public void EnumerableFirstOrNoneDemo()
        {
            Console.WriteLine("*** IEnumerable.FirstOrNone() demo:");
            IEnumerable<Color> colors = new[]
            {
                Color.Red, Color.Blue
            };

            Option<Color> color1 = new Color[0].FirstOrNone(); // None
            Option<Color> color2 = colors.FirstOrNone(); // Some(Red)
            Option<Color> color3 = colors.FirstOrNone(c => c == Color.Green); // None

            Console.WriteLine($"{color1}, {color2}, {color3}");
        }

        [Fact]
        public void EnumerableSelectOptionalDemo()
        {
            Console.WriteLine("*** IEnumerable.SelectOptional() demo:");

            IEnumerable<Person> people = new[]
            {
                new Person("Jack", 9, Color.Green), // No car
                new Person("Jill", 19, Color.Red), // Has a red car
                new Person("Joe", 22, Color.Blue) // Has a blue car
            };

            IEnumerable<Color> carColors =
                people.SelectOptional(person => person.TryGetCar())
                    .Select(car => car.Color); // [Red, Blue]

            Console.WriteLine(string.Join(", ", carColors.Select(c => c.Label).ToArray()));
        }

        [Fact]
        public void DictionaryTryGetValueDemo()
        {
            Console.WriteLine("*** IDictionary.TryGetValue() demo:");

            IEnumerable<Person> people = new[]
            {
                new Person("Jack", 9, Color.Green), // No car
                new Person("Jill", 19, Color.Red), // Has a red car
                new Person("Joe", 22, Color.Blue) // Has a blue car
            };

            IDictionary<string, Car> nameToCar = people // IEnumerable<Person>
                .SelectOptional(person =>
                        person.TryGetCar() // Option<Car>
                            .Map(car => (name: person.Name, car: car)) // Option<(string, Car)>
                ) // IEnumerable<(string, Car)>
                .ToDictionary(
                    tuple => tuple.name, //   key = name
                    tuple => tuple.car); // value = car

            Console.WriteLine(nameToCar.TryGetValue("Jill").ToString()); // Prints Some
            Console.WriteLine(nameToCar.TryGetValue("Jimmy").ToString()); // Prints None
        }

        [Fact]
        public void OptionEqualityDemo()
        {
            Console.WriteLine("*** Option.Equals() demo:");

            Option<Color> redOption = Color.Red;
            Option<Color> equalRedOption = Color.Red;
            Option<Color> blueOption = Color.Blue;

            Console.WriteLine($"HashCode {redOption.GetHashCode()} : {equalRedOption.GetHashCode()}");
            Assert.Equal(redOption.GetHashCode(), equalRedOption.GetHashCode());

            Console.WriteLine($"{redOption} {(redOption.Equals(equalRedOption) ? "==" : "!=")} {equalRedOption}");
            Assert.Equal(redOption, equalRedOption);
            Console.WriteLine($"{redOption} {(redOption.Equals(Color.Red) ? "==" : "!=")} {Color.Red}");
            Assert.NotSame(redOption, Color.Red);
            Console.WriteLine($"{Color.Red} {(Color.Red.Equals(redOption) ? "==" : "!=")} {redOption}");
            Assert.NotSame(Color.Red, redOption);
            Console.WriteLine($"{redOption} {(redOption.Equals(blueOption) ? "==" : "!=")} {blueOption}");
            Assert.NotEqual(redOption, blueOption);

            Option<Color> noneOption = None.Value;
            Option<Color> alsoNoneOption = None.Value;

            Console.WriteLine($"{noneOption} {(noneOption.Equals(alsoNoneOption) ? "==" : "!=")} {alsoNoneOption}");
            Assert.Equal(noneOption, alsoNoneOption);
            Console.WriteLine($"{noneOption} {(noneOption.Equals(None.Value) ? "==" : "!=")} {None.Value}");
            Assert.Equal(None.Value, noneOption);
            Console.WriteLine($"{None.Value} {(None.Value.Equals(noneOption) ? "==" : "!=")} {noneOption}");
            Assert.Equal(None.Value, noneOption);

            List<string> list = new List<string>() {"a", "b", "c"};
            string listString = "[" + string.Join(", ", list.ToArray()) + "]";
            Console.WriteLine($"{None.Value} {(None.Value.Equals(list) ? "==" : "!=")} {listString}");
            Assert.NotSame(None.Value, list);
            Console.WriteLine($"{listString} {(list.Equals(None.Value) ? "==" : "!=")} {None.Value}");
            Assert.NotSame(list, None.Value);
            Console.WriteLine($"{noneOption} {(noneOption.Equals(list) ? "==" : "!=")} {listString}");
            Assert.NotSame(noneOption, list);
            Console.WriteLine($"{listString} {(list.Equals(noneOption) ? "==" : "!=")} {None.Value}");
            Assert.NotSame(list, noneOption);
        }

        [Fact]
        public void OptionalInterfaceDemo()
        {
            Console.WriteLine("*** Optional Interface Demo:");

            Person jill = new Person("Jill", 19, Color.Red); // Has a car
            Option<Car> jillsCar = jill.TryGetCar(); // Some

            if (jillsCar is Some<Car> some1)
            {
                PrintName(some1);
            }

            Option<ICar> abstractCar =
                jill.TryGetCar().OfType<ICar>(); // Some<ICar>

            if (abstractCar is Some<ICar> someOther)
            {
                PrintAbstractName(someOther.Content);
            }
        }

        private void PrintName(Car car)
        {
            Console.WriteLine(car.Name);
        }

        private void PrintAbstractName(ICar car)
        {
            Console.WriteLine(car.Name);
        }
    }
}
