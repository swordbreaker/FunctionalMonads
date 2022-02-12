using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.ParserMonad;
using FunctionalMonadsExamples.Models.Operations;

namespace FunctionalMonadsExamples.Parsers
{
    internal class CalculationParser
    {
        private readonly char[] _supportedOperations = new char[] { '+', '-', '*', '/', '^'};

        public IEither<IOperation<decimal>, IParseFailure> Parse(string text) =>
            RootParser.ParseToValue(text);

        private IParser<IOperation<decimal>> RootParser => 
            from op in OperationParser
            from eof in Consume.EndOfString
            select op;

        private IParser<IOperation<decimal>> OperationParser =>
            from a in Consume.Decimal
            from op in Consume.Char(_supportedOperations)
            from b in Consume.Decimal
            select MapToOperation(a, b, op);

        private IOperation<decimal> MapToOperation(decimal a, decimal b, char op) =>
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
