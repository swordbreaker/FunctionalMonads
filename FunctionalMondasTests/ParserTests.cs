using FluentAssertions;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.MaybeMonad;
using FunctionalMonads.Monads.ParserMonad;
using NUnit.Framework;

namespace FunctionalMonadsTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TokenTest()
        {
            var input = "  Token  ";

            var parser = Consume.String("Token").Token();

            var output = parser.Parse(input);

            var left = output.Should().BeAssignableTo<Left<IPResult<string>>>().Subject;
            left.Value.Start.Column.Should().Be(0);
            left.Value.Next.Column.Should().Be(input.Length);
            left.Value.Value.Should().Be("Token");
        }
    }
}