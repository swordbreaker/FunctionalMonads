using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonads.Monads.ParserMonad
{
    public record TextPoint(char Current, int Line, int Column)
    {
        private readonly int _position;
        private readonly string _text;

        public static TextPoint Create(string content) =>
            new(0, content + '\0', 0, 0);

        private TextPoint(int position, string text, int line, int column)
            :this(text[position], line, column)
        {
            _position = position;
            _text = text;
        }

        public IMaybe<TextPoint> Advance()
        {
            return CanAdvance
                ? Maybe.Some(
                    new TextPoint(
                        _position + 1, _text,
                        IsNewLine ? Line + 1 : Line,
                        IsNewLine ? 0 : Column + 1))
                : Maybe.None<TextPoint>();
        }

        public bool CanAdvance => _position < _text.Length - 1;

        private bool IsNewLine => Current == '\n';

        public override string ToString()
        {
            return $"{Column}:{Line}";
        }
    }
}