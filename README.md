# FunctionalMonads
A simple library for monads in C#

The libary constis of two new Monads and some extention methods.

## What is a monad?
A monad is a abstract structure for TODO

## Maybe
The Maybe monad is helpful, when a value can have some value or can be none.

Create a Maybe with value.
```csharp
// create some maybe
IMaybe<int> maybe = Maybe.Some(5);
```
Create a Maybe without a value.
```csharp
// create none maybe 
IMaybe<int> maybe = Maybe.None<int>();
```

Check the maybe with pattern matching
```csharp
if (maybe is Some<int> some)
{
    Console.WriteLine(some.Value);
}
else
{
    Console.WriteLine(0);
}
```
Or with:
```csharp
var y = maybe switch
{
    Some<int> some => some.Value,
    None<int> _ => 0,
    _ => throw new ArgumentOutOfRangeException(nameof(maybe))
};
```
Or:
```csharp
var y = maybe.Match(x => x, () => 0);
```
The Some struct is implicit castable to the underling type.
```csharp
var some = new Some<int>(42);
int i = some;
```
Map for example `IMaybe<int>` to `IMaybe<string>`.
```csharp
var maybe = Maybe.Some(5);
var textMaybe = maybe.Map(i => i.ToString());
```
If you have a for example these two Methods.
```csharp
public IMaybe<Person> GetPersonFromAddress(Address address) =>
    Maybe.Some(new Person("Hans", address));

public IMaybe<Car> GetCarFromPerson(Person person) =>
    person.Name == "Hans"
        ? Maybe.Some(new Car(person, "BMW"))
        : Maybe.None<Car>();
```
You could retriev the car model as follows:
```csharp
public IMaybe<string> GetModel(Address address)
{
    var person = GetPersonFromAddress(address);
    if (person is Some<Person> somePerson)
    {
        var car = GetCarFromPerson(somePerson);
        if (car is Some<Car> someCar)
        {
            return Maybe.Some(someCar.Value.Model);
        }
    }

    return Maybe.None<string>;
}
```
Or you could use Bind which does a Map from `IMaybe<T>` to `IMaybe<IMaybe<K>>` and the flatten it to `IMaybe<K>`.
```csharp
public IMaybe<string> GetModel(Address address)
{
    IMaybe<Person> person = GetPersonFromAddress(address);
    IMaybe<Car> car = person.Bind(GetCarFromPerson);
    return car.Map(c => c.Model);
}
```
Or even better use the `for .. in` syntax.
```csharp
public IMaybe<string> GetModel(Address address) =>
    from person in GetPersonFromAddress(address)
    from car in GetCarFromPerson(person)
    select car.model;
```

## Either
The Either Monad is helpful when a return value can be either of a type a or a type b.

Create a Either Monad with a left value.
```csharp
var left = Either.Left<string, double>("left value");
```
Create a Either Monad with a right value.
```csharp
var right = Either.Left<string, int>(42);
```
You can also use Pattern matching
```csharp
var value = either switch
{
    Left<string> l => l,
    Right<int> i => i.ToString(),
    _ => throw new ArgumentOutOfRangeException(nameof(either))
};
```