using FluentAssertions;
using FunctionalMonads.Monads.EitherMonad;
using FunctionalMonads.Monads.ParserMonad;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;

namespace FunctionalMondasTests
{
    public class ConsumeTests
    {
        [TestCase("5", 5)]
        [TestCase("-3", -3)]
        public void TestValidInt(string input, int number)
        {
            var result = Consume.Int.Parse(input);
            result.Do(
                x => x.Value.Should().Be(number),
                f => Assert.Fail(f.Message));
        }

        [TestCase("5.0")]
        [TestCase("--5")]
        [TestCase("+5")]
        [TestCase("a")]
        [TestCase("5a")]
        [TestCase("a5")]
        public void TestInvalidInt(string input)
        {
            var result = Consume.Int.Then(Consume.EndOfLine).Parse(input);
            result.IsRight.Should().BeTrue();
        }

        [TestCase("5.0", 5.0)]
        [TestCase("-3.0", -3.0)]
        [TestCase(".2", 0.2)]
        public void TestValidDouble(string input, double number)
        {
            var result = Consume.Double.Parse(input);
            result.Do(
                x => x.Value.Should().Be(number),
                f => Assert.Fail(f.Message));
        }

        [TestCase("a", 'a')]
        [TestCase("c", 'c')]
        [TestCase("d", 'd')]
        public void TestMultipleCharactersChar(string input, char expectedOutput)
        {
            var result = Consume.Char('a', 'b', 'c', 'd').Parse(input);

            result.IsLeft.Should().BeTrue();
            result.Do(
                x => x.Value.Should().Be(expectedOutput),
                f => Assert.Fail(f.Message));
        }
    }
}
