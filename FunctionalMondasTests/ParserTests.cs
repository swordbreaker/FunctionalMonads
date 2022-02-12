using System;
using System.Collections;
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
        public void Test()
        {
            var text = "aaa";

            var parser = Consume.Char('a').Many();
            var result = parser.Parse(TextPoint.Create(text));

            result.IsLeft.Should().BeTrue();

            var p = parser.Map(chars => chars.AsString());

            var r = p.Parse(TextPoint.Create(text));
            r.Do(
                pResult => pResult.Value.Should().Be(text),
                failure => Assert.Fail("Result should be successful"));
        }

        [TestCase("-5", -5)]
        public void Bla(string text, int value)
        {
            var parser =
                from minus in Consume.Char('-').Optional()
                from number in Consume.Char(char.IsDigit, "Digit")
                select int.Parse($"{minus.SomeOrProvided(' ')}{number}");

            var result = parser.Parse(text);

            result.Do(
                pResult =>
                {
                    pResult.Value.Should().Be(value);
                    pResult.Start.Column.Should().Be(0);
                    pResult.Next.Column.Should().Be(2);
                }, failure => Assert.Fail(failure.Message));
        }

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