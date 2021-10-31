using System;
using System.Collections;
using FluentAssertions;
using FunctionalMonads.Monads.EitherMonad;
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

        [Test]
        public void Bla()
        {
            var text = "-5";

            var parser =
                from minus in Consume.Char('-')
                from number in Consume.Char(char.IsDigit, "Digit")
                select int.Parse($"{minus}{number}");

            var result = parser.Parse(text);

            result.Do(
                pResult =>
                {
                    pResult.Value.Should().Be(-5);
                    pResult.Start.Column.Should().Be(0);
                    pResult.End.Column.Should().Be(1);
                }, failure => Assert.Fail(failure.Message));

        }

        [Test]
        public void TokenTest()
        {
            var input = "  Token  ";

            var parser = Consume.String("Token").Token();

            var output = parser.Parse(input);

            var left = output.Should().BeAssignableTo<Left<IPResult<string>>>().Subject;
            left.Value.Start.Column.Should().Be(3);
            left.Value.End.Column.Should().Be(3 + "Token".Length);
            left.Value.Value.Should().Be("Token");

            input.Substring(left.Value.Start.Column, "Token".Length).Should().Be("Token");
        }
    }
}