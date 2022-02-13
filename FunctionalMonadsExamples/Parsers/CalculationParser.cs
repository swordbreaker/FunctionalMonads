using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.ParserMonad;
using FunctionalMonadsExamples.Models.Operations;
using FunctionalMonadsExamples.Models.Statement;

namespace FunctionalMonadsExamples.Parsers
{
    internal class CalculationParser
    {
        private static readonly char[] _supportedOperations = new char[] { '+', '-', '*', '/', '^'};

        public IEither<IStatement, IParseFailure> Parse(string text) =>
            RootParser.ParseToValue(text);

        private static IParser<IStatement> RootParser =>
            from statement in TimeParser | OperationParser | ExitParser
            from eof in Consume.EndOfInput
            select statement;

        private static IParser<IStatement> TimeParser =>
            Consume.String("Time", caseSensitve: false)
                .Select(e => new TimeStatement());

        private static IParser<IStatement> ExitParser =>
            (Consume.String("quit", caseSensitve: false) | Consume.String("exit", caseSensitve: false))
            .Select(e => new ExitStatement());

        private static IParser<DecimalOperation> OperationParser =>
            from a in Consume.Decimal
            from op in Consume.Char(_supportedOperations)
            from b in Consume.Decimal
            select MapToOperation(a, b, op);

        private static DecimalOperation MapToOperation(decimal a, decimal b, char op) =>
            op switch
            {
                '+' => new AddOperations(a, b),
                '-' => new SubstractOperations(a, b),
                '*' => new MultiplicationOperations(a, b),
                '/' => new DivideOperations(a, b),
                '^' => new PowerOfOperations(a, b),
                _ => throw new ArgumentOutOfRangeException(nameof(op)),
            };
    }
}
