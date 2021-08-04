using System;
using System.Collections;
using FunctionalMonads.Monads.EitherMonad;

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
                                new PResult<char>(point.Current)),
                        () => 
                            Either.Right<IPResult<char>, IParseFailure>(
                                new ParseFailure(point, "Unexpected end of line.")));

                return Either.Right<IPResult<char>, IParseFailure>(
                    new ParseFailure(point, $"Expected ${charDescription} got {point.Current}."));
            });

        public static IParser<char> Char(char character) =>
            Char(c => c == character, character.ToString());
    }
}