# FunctionalMonads
A simple library for monads in C#

The library constis of three Monads (Maybe, Either and Parser) and some extention methods.

## What is a monad?
A monad is a abstract structure with one or more inner types. For the structure to be called a monad is needs to support a set of functions:
1. Construction: A way of elevating a primitive type to an Monad.
```csharp
return :: a -> Monad a

Monad<int> monda = New(5);
```
2. The map function accepts a Function which acts on the primitve type and applys a transformation.
```csharp
map :: (a -> b) -> Monad a -> Monad b

Monad<int> monadA = New(5);
Monad<string> monadB monadA.Map(x => x.ToString())
```
3. The flatten function
```csharp
flatten :: Monad Monad a -> Monad a

Monad<Monad<int>> monda = New(New(5));
Monad<int> flattend = monad.Flatten();
```
4. The bind function which is a map followed by a flatten
```csharp
bind :: Monad a -> (a -> Monad b) -> Monad b

Monad<int> monadA = New(5)
Monad<double> mondaB = monadA.Bind(x => New(x/2.0)) 
```
In C# the LINQ provides for `IEnumerable` the monad functionality.
1. Construction
```csharp
var seq = Enumerable.Range(1, 5);
```
2. Map
```csharp
var seq2 = seq.Select(x => x*2);
```
3. Flatten
```csharp
IEnumerable<IEnumerable<int>> seq = Enumerable.Repeat(Enumerable.Range(1, 5), 3);
IEnumerable<int> seq2 = seq.SelectMany(x => x);
```
4. Bind
```csharp
var seq = Enumerable.Range(1, 3);
var seq2 = seq.SelectMany(i => Enumerable.Repeat(i, 2));
Console.WriteLine(string.Join(",", seq2)); // 1,1,2,2,3,3
```
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
var right = Either.Right<string, int>(42);
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

## Task as Monad
A C\# Task is very simialar to a Monad but does not support all needed functionality, therefore this library provides three extention methods.

Map
```csharp
var taskNumber = Task.FromResult(5);
var result = await taskNumber.Map(i => i * 2);
Debug.WriteLine(result); // 10
```
Flatten
```csharp
var nested = Task.FromResult(Task.FromResult(1));
var result = await nested.Flatten();
Debug.WriteLine(result); // 1
```
Bind
```csharp
Task<string> AddWorld(string a) => 
    Task.FromResult(a + " World");

var firstTask = Task.FromResult("Hello");
var result = await firstTask.Bind(AddWorld);
Debug.WriteLine(result); // Hello World
```

## Parser
A simple parser is also provided. It provied a parse method which retunrs an Either Monad where Left is `IPResult<T>` and right `IParseFailure`.
```csharp
var result = Consume.Int.Parse("5");

result
    .IfLeft(success => 
        Console.WriteLine($"Parsed {success.Value} at {success.Start.Column}:{success.Start.Line}"));
// Will output: Parsed 5 at 0:0
```
The Parse also supports the `for .. in` syntax.
```csharp
var pointParser =
    from prefix in Consume.Char('P')
    from openBrace in Consume.Char('(')
    from numbers in Consume.Int.Token().OneOrMore()
    from closeBrace in Consume.Char(')')
    select numbers.ToArray();

var result = pointParser.ParseToValue("P(1 2 3)");
result.IfLeft(x => Console.WriteLine(string.Join(',', x)));
// Will output: 1,2,3

var failure = pointParser.ParseToValue("P1 2 3)");
failure.IfRight(f => Console.WriteLine(f.Message));
// Will output: Expected ( got 1.
```