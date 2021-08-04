using System;
using FunctionalMonads.Monads.EitherMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public class Parser<T> : IParser<T>
    {
        public delegate IEither<IPResult<K>, IParseFailure> ParseFuc<out K>(TextPoint point);

        private readonly ParseFuc<T> _parserFunc;

        public Parser(ParseFuc<T> parserFunc)
        {
            _parserFunc = parserFunc;
        }

        public IEither<IPResult<T>, IParseFailure> Parse(TextPoint point) => 
            _parserFunc(point);

        public IParser<TMap> Map<TMap>(Func<IPResult<T>, TMap> mapFunc) => 
            new Parser<TMap>(
                point => 
                    _parserFunc(point)
                        .MapLeft(result => 
                            new PResult<TMap>(mapFunc(result))));

        public static implicit operator Parser<T>(ParseFuc<T> parseFuc) =>
            new(parseFuc);

        //public IParser<TBind> Bind<TBind>(Func<IPResult<T>, IParser<TBind>> binFunc) =>
        //    new Parser<TBind>(point => 
        //        _parserFunc(point)
        //            .BindLeft(result => binFunc(result).));

        public IParser<TBind> Bind<TBind>(Func<IPResult<T>, IParser<TBind>> binFunc) =>
            throw new NotImplementedException();
    }
}