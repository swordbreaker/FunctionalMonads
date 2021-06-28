# FunctionalMonads
A simple library for monads in C#

The libary constis of two new Monads and some extention methods.

## What is a monad?
A monad is a abstract structure for 

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

```csharp
public IMaybe<Person> GetPersonFromAddress(Address address) =>
    Maybe.Some(new Person("Hans", address));

public IMaybe<Car> GetCarFromPerson(Person person) =>
    person.Name == "Hans"
        ? Maybe.Some(new Car(person, "BMW"))
        : Maybe.None<Car>();
```

```csharp
public IMaybe<string> GetModel(Address address)
{
    var person = GetPersonFromAddress(address);
    if (person is Some<Person> somePerson)
    {
        var car = GetCarFromPerson(somePerson);
        if (car is Some<Car> someCar)
        {
            return Maybe.Some(someCar.Value);
        }
    }

    return Maybe.None<string>;
}
```

```csharp
public IMaybe<string> GetModel(Address address)
{
    IMaybe<Person> person = GetPersonFromAddress(address);
    IMaybe<Car> car = person.Bind(GetCarFromPerson);
    return car.Map(c => c.model);
}
```

```csharp
public IMaybe<string> GetModel(Address address) =>
    from person in GetPersonFromAddress(address)
    from car in GetCarFromPerson(person)
    select car.model;
```