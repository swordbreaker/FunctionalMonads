using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.ParserMonad;
using FunctionalMonadsExamples.Models.Statement;
using FunctionalMonadsExamples.Parsers;

var parser = new CalculationParser();

Console.WriteLine("Write a statement.");

IEither<IStatement, IParseFailure> result;
do
{
    var line = Console.ReadLine();

    result = parser.Parse(line ?? string.Empty);
    result
        .MapLeft(x => x.Evaluate())
        .Do(
            m => Console.WriteLine(m),
            f => Console.WriteLine(f.Message));
} while (!result.Match(x => x is ExitStatement, f => false));