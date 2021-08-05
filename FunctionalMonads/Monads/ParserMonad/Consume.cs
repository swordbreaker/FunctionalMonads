using System;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public static class Consume
    {
        public static IParser<char> Char(Predicate<char> predicate, string charDescription) =>
            new Parser<char>(point =>
            {
                if (predicate(point.Current))
                    return point.Advance().Match(
                        textPoint => 
                            Either.Left<IPResult<char>, IParseFailure>(
                                new PResult<char>(point.Current, point, textPoint)),
                        () => 
                            Either.Right<IPResult<char>, IParseFailure>(
                                new ParseFailure(point, point, "Unexpected end of line.")));

                return Either.Right<IPResult<char>, IParseFailure>(
                    new ParseFailure(point, point, $"Expected {charDescription} got {point.Current}."));
            });

        public static IParser<char> Char(char character) =>
            Char(c => c == character, character.ToString());

        public static IParser<string> String(string text) =>
            new Parser<string>(point =>
            {
                var current = point;
                var i = 0;

                while (i < text.Length && point.Advance() is Some<TextPoint> next && current.Current == text[i])
                {
                    i++;
                    current = next.Value;
                }

                return (i == text.Length)
                    ? Either.Left<IPResult<string>, IParseFailure>(
                        new PResult<string>(text, point, current))
                    : Either.Right<IPResult<string>, IParseFailure>(
                        new ParseFailure(point, current, $"Expected {text[i]} got {current.Current}"));
            });

        public static IParser<char> Digit =>
            Char(char.IsDigit, "digit");

        public static IParser<char> Whitespace =>
            Char(char.IsWhiteSpace, "whitespace");

        public static IParser<string> EndOfLine =>
            String(Environment.NewLine);

        public static IParser<uint> UInt =>
            Digit.Many().Map(result => uint.Parse(result.Value.AsString()));

        public static IParser<int> Int =>
            from minus in Char('-').Optional()
            from digits in Digit.Many()
            select minus.Value.Match(
                c => -int.Parse(digits.Value.AsString()),
                () => int.Parse(digits.Value.AsString()));

        public static IParser<double> Double =>
            from characteristic in Int
            from fractional in Char('.').Then(Digit.Many()).Optional()
            select characteristic.Value + fractional.Value.Match(
                c => double.Parse($"0{c}"),
                () => 0);

    }
}