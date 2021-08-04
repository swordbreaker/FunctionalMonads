using FluentAssertions;
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

            var p = parser.Map(pResult => pResult.Value.AsString());
            var r = p.Parse(TextPoint.Create(text));
            r.Do(
                pResult => pResult.Value.Should().Be(text),
                failure => Assert.Fail("Result should be successful"));
        }
    }
}