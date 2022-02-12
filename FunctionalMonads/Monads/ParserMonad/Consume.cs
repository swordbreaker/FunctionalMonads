using System;
using System.Linq;
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

        public static IParser<char> Char(params char[] characters) =>
            new Parser<char>(point =>
                characters
                    .Select(c => Char(c).Parse(point))
                    .FirstOrNone(r => r.IsLeft)
                    .SomeOrProvided(() =>
                        Either.Right<IPResult<char>, IParseFailure>(
                            new ParseFailure(point, point, $"Expected one of {string.Join(',', characters)} got {point.Current}"))));

        public static IParser<string> String(string text, string textDescription = null) =>
            new Parser<string>(point =>
            {
                string GetDesciption(int i) =>
                    textDescription ?? text[i].ToString();

                var current = point;
                var i = 0;

                while (i < text.Length && current.Advance() is Some<TextPoint> next && current.Current == text[i])
                {
                    i++;
                    current = next.Value;
                }

                return (i == text.Length)
                    ? Either.Left<IPResult<string>, IParseFailure>(
                        new PResult<string>(text, point, current))
                    : Either.Right<IPResult<string>, IParseFailure>(
                        new ParseFailure(point, current, $"Expected {GetDesciption(i)} got {current.Current}"));
            });

        public static IParser<char> Digit =>
            Char(char.IsDigit, "digit");

        public static IParser<char> Whitespace =>
            Char(char.IsWhiteSpace, "whitespace");

        public static IParser<string> EndOfLine =>
            String(Environment.NewLine, "End Of Line");

        public static IParser<Unit> EndOfString =>
            new Parser<Unit>(point => point.CanAdvance
                ? Either.Right<IPResult<Unit>, IParseFailure>(
                    new ParseFailure(point, point, $"Expected EOF got {point.Advance().Map(x => x.Current).SomeOrProvided(' ')}"))
                : Either.Left<IPResult<Unit>, IParseFailure>(
                    new PResult<Unit>(new Unit(), point, point)));

        public static IParser<uint> UInt =>
            Digit.Many().Map(result => uint.Parse(result.Value.AsString()));

        public static IParser<int> Int =>
            from minus in Char('-').Optional()
            from digits in Digit.OneOrMore()
            select minus.Match(
                c => -int.Parse(digits.AsString()),
                () => int.Parse(digits.AsString()));

        public static IParser<double> Double =>
            FloatParser
                .Map(t => t.characteristic + double.Parse($"0.{t.fractional}"));

        public static IParser<decimal> Decimal =>
            FloatParser
                .Map(t => t.characteristic + decimal.Parse($"0.{t.fractional}"));

        private static IParser<(int characteristic, string fractional)> FloatParser =>
            new Parser<(int characteristic, string fractional)>(point =>
            {
                (int characteristic, string fractional) UnpackMaybe((IMaybe<int> characteristic, IMaybe<string> fractional) t) =>
                    (t.characteristic.SomeOrProvided(0),
                                 t.fractional
                                     .Map(x => x.AsString())
                                     .SomeOrProvided("0"));

                var parser = from characteristic in Int.Optional()
                             from fractional in Char('.').Then(Digit.OneOrMore()).Optional()
                             select (characteristic, fractional: fractional.Map(x => x.AsString()));

                return parser.Parse(point).BindLeft(x => 
                    x.Value.characteristic.IsNone && x.Value.fractional.IsNone
                        ? Either.Right<IPResult<(int characteristic, string fractional)>, IParseFailure>(
                            new ParseFailure(point, point, $"Expected Number got {point.Current}"))
                        : Either.Left<IPResult<(int characteristic, string fractional)>, IParseFailure>(
                            new PResult<(int characteristic, string fractional)>(UnpackMaybe(x.Value), point, x.Next))
                );
            });
    }
}