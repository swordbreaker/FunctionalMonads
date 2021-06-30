using FunctionalMonads.Monads.MaybeMonad;
using NUnit.Framework;

namespace FunctionalMonadsTests
{
    public class MaybeIntegrationTests
    {
        [Test]
        public IMaybe<string> GetModel(Address address)
        {
            IMaybe<Person> person = GetPersonFromAddress(address);
            IMaybe<Car> car = person.Bind(p => GetCarFromPerson(p));
            return car.Map(c => c.model);
        }

        //from person in GetPersonFromAddress(address)

            //var person = GetPersonFromAddress(address);
            //if (person is MaybeSome<Person> somePerson)
            //{
            //    var car = GetCarFromPerson(somePerson);
            //    if (car is MaybeSome<Car> someCar)
            //    {
            //        return someCar.Value.model;
            //    }
            //}

            //return "No Car found";


        public record Address();

        public record Person(string Name, Address Address);

        public record Car(Person Person, string model);

        public IMaybe<Person> GetPersonFromAddress(Address address) =>
            Maybe.Some(new Person("Hans", address));

        public IMaybe<Car> GetCarFromPerson(Person person) =>
            person.Name == "Hans"
                ? Maybe.Some(new Car(person, "BMW"))
                : Maybe.None<Car>();


        public IMaybe<int> Div(int a, int b)
        {
            if (b == 0)
            {
                return Maybe.None<int>();
            }

            return Maybe.Some(a / b);
        }
    }
}