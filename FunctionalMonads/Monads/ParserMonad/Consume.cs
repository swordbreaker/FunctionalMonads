using System;
using System.Collections.Generic;
using System.Linq;
using Dawn;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public static class Consume
    {
        /// <summary>
        /// Consume a character.
        /// </summary>
        /// <param name="predicate">Predicate to determine if the the caracter will be consumed.</param>
        /// <param name="charDescription">The description of the character to consume, used for giving a reasonable falure message.</param>
        /// <returns>A new parser.</returns>
        public static IParser<char> Char(Predicate<char> predicate, string charDescription) =>
            new Parser<char>(point =>
            {
                if (predicate(point.Current)) 
                {
                    return point.Advance().Match(
                        onSome: textPoint =>
                            Success(point.Current, point, textPoint),
                        onNone: () => 
                            Failure<char>(point, point, "Unexpected end of string."));
                }

                return Failure<char>(point, point, $"Expected {charDescription} got {point.Current}.");
            });

        /// <summary>
        /// Consume a character.
        /// </summary>
        /// <param name="character">Character to consume.</param>
        /// <returns>A new parser.</returns>
        public static IParser<char> Char(char character) =>
            Char(c => c == character, character.ToString());

        /// <summary>
        /// Consumes a character.
        /// </summary>
        /// <param name="characters">An array of possible character.</param>
        /// <returns>A new parser. </returns>
        public static IParser<char> Char(params char[] characters) =>
            new Parser<char>(point =>
                characters
                    .Select(c => Char(c).Parse(point))
                    .FirstOrNone(r => r.IsLeft)
                    .SomeOrProvided(() =>
                        Failure<char>(point, point, $"Expected one of {string.Join(',', characters)} got {point.Current}")));

        /// <summary>
        /// Consumes a string.
        /// </summary>
        /// <param name="text">The text to consume.</param>
        /// <param name="textDescription">Optional description of the text. If null the description will be equal to text.</param>
        /// <param name="caseSensitve">If the match is case sensitve or not.</param>
        /// <returns>A new parser.</returns>
        public static IParser<string> String(string text, string textDescription = null, bool caseSensitve = true) =>
            new Parser<string>(point =>
            {
                string GetDesciption(int i) =>
                    textDescription ?? text[i].ToString();

                char Process(char c) =>
                    caseSensitve ? c : char.ToLower(c);

                var current = point;
                var i = 0;

                while (i < text.Length && current.Advance() is Some<TextPoint> next && Process(current.Current) == Process(text[i]))
                {
                    i++;
                    current = next.Value;
                }

                return (i == text.Length)
                    ? Success(text, point, current)
                    : Failure<string>(point, current, $"Expected {GetDesciption(i)} got {current.Current}");
            });

        /// <summary>
        /// Consume a digit see <see cref="char.IsDigit(char)"/>
        /// </summary>
        public static IParser<char> Digit =>
            Char(char.IsDigit, "digit");

        /// <summary>
        /// Consume a whitespace see <see cref="char.IsWhiteSpace(char)"/>
        /// </summary>
        public static IParser<char> Whitespace =>
            Char(char.IsWhiteSpace, "whitespace");

        /// <summary>
        /// Consume a end of line.
        /// </summary>
        public static IParser<string> EndOfLine =>
            String(Environment.NewLine, "End Of Line");

        /// <summary>
        /// Consume the end of the input.
        /// </summary>
        public static IParser<Unit> EndOfInput =>
            new Parser<Unit>(point => point.CanAdvance
                ? Failure<Unit>(point, point, $"Expected EOF got {point.Advance().Map(x => x.Current).SomeOrProvided(' ')}")
                : Success(new Unit(), point, point));

        /// <summary>
        /// Consume a unit
        /// </summary>
        public static IParser<uint> UInt =>
            new Parser<uint>(point =>
                Digit.Many().Parse(point)
                    .BindLeft(result => TryParse<uint>(result, uint.TryParse)));

        /// <summary>
        /// Cosnume an integer.
        /// </summary>
        public static IParser<int> Int =>
            new Parser<int>(point =>
            {
                var parser = from minus in Char('-').Optional()
                             from digits in Digit.OneOrMore()
                             select minus.Match(
                                 x => x + digits.AsString(),
                                 () => digits.AsString());

                return parser.Parse(point).BindLeft(result => TryParse<int>(result, int.TryParse));
            });

        /// <summary>
        /// Consume a double.
        /// </summary>
        public static IParser<double> Double =>
            new Parser<double>(point =>
                FloatParser
                    .Map(t => t.characteristic + $".{t.fractional}")
                    .Parse(point)
                    .BindLeft(result => TryParse<double>(result, double.TryParse)));

        /// <summary>
        /// Consume a decimal.
        /// </summary>
        public static IParser<decimal> Decimal =>
            new Parser<decimal>(point =>
                FloatParser
                    .Map(t => t.characteristic + $".{t.fractional}")
                    .Parse(point)
                    .BindLeft(result => TryParse<decimal>(result, decimal.TryParse)));

        private static IParser<(int characteristic, string fractional)> FloatParser =>
            new Parser<(int characteristic, string fractional)>(point =>
            {
                static (int characteristic, string fractional) UnpackMaybe((IMaybe<int> characteristic, IMaybe<string> fractional) t) =>
                    (t.characteristic.SomeOrProvided(0),
                                 t.fractional
                                     .Map(x => x.AsString())
                                     .SomeOrProvided("0"));

                var parser = from characteristic in Int.Optional()
                             from fractional in Char('.').Then(Digit.OneOrMore()).Optional()
                             select (characteristic, fractional: fractional.Map(x => x.AsString()));

                return parser.Parse(point).BindLeft(x => 
                    x.Value.characteristic.IsNone && x.Value.fractional.IsNone
                        ? Failure<(int characteristic, string fractional)>(point, point, $"Expected Number got {point.Current}")
                        : Success(UnpackMaybe(x.Value), point, x.Next));
            });


        /// <summary>
        /// Try out one parser after another first who succeeds will be apply and returned.
        /// If none succeeds returns a Failure.
        /// </summary>
        /// <typeparam name="T">The inner type of the parsers.</typeparam>
        /// <param name="parsers">The parsers.</param>
        /// <returns>A new parser.</returns>
        public static IParser<T> Or<T>(params IParser<T>[] parsers) =>
            new Parser<T>(point =>
            {
                var result = parsers[0].Parse(point);
                int i = 1;

                while (i < parsers.Length && result.IsRight)
                {
                    result = parsers[i++].Parse(point);
                }

                return result;
            });

        private static IEither<IPResult<T>, IParseFailure> Success<T>(T value, TextPoint point, TextPoint next) =>
            Either.Left<IPResult<T>, IParseFailure>(
                new PResult<T>(value, point, next));

        private static IEither<IPResult<T>, IParseFailure> Failure<T>(TextPoint point, TextPoint end, string message) =>
            Either.Right<IPResult<T>, IParseFailure>(
                new ParseFailure(point, end, message));

        private delegate bool TryParseHandler<T>(string value, out T result);

        private static IEither<IPResult<T>, IParseFailure> TryParse<T>(IPResult<IEnumerable<char>> result, TryParseHandler<T> tryPareseFunction)
        {
            Guard.Argument(tryPareseFunction, nameof(tryPareseFunction)).NotNull();

            return tryPareseFunction(result.Value.AsString(), out T value)
                ? Success(value, result.Start, result.Next)
                : Failure<T>(result.Start, result.Start, $"Cannot parse {value} into an unit");
        }
    }
}