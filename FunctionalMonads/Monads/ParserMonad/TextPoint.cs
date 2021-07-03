using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public record TextPoint(char Current, int Line, int Column)
    {
        private readonly int _position;
        private readonly string _text;

        private TextPoint(int position, string text, int line, int column)
            :this(text[position], line, column)
        {
            _position = position;
            _text = text;
        }

        public IMaybe<TextPoint> Advance()
        {
            if (_position < _text.Length)
            {
                return Maybe.Some(
                    new TextPoint(
                    _position + 1, _text, 
                    IsNewLine ? 0 : Column + 1,
                    IsNewLine ? Line + 1 : Line));
            }

            return Maybe.None<TextPoint>();
        }

        private bool IsNewLine => Current == '\n';
    }
}