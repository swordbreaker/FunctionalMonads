using System.Linq;
using FluentAssertions;
using FunctionalMonads.Monads.EnumerableMonad;
using NUnit.Framework;

namespace FunctionalMonadsTests
{
    public class EnumerableMonadTests
    {
        [Test]
        public void HeadTailMatchTest()
        {
            var sut = new[]
            {
                1,
                2,
                3,
            };

            var result= sut.Match(
                (h, b) => b.Append(h))
                .ToList();

            result[0].Should().Be(sut[1]);
            result[1].Should().Be(sut[2]);
            result.Last().Should().Be(result[0]);
        }

        [Test]
        public void HeadTailMatchOneElementTest()
        {
            var sut = new[]
            {
                1,
            };

            var result = sut.Match(
                (head, tail) => tail.Append(head))
                .ToList();

            result[0].Should().Be(sut[0]);
        }

        [Test]
        public void InitTailMatchTest()
        {

            var sut = new[]
            {
                1,
                2,
                3,
            };

            var result = sut.Match(
                (init, last) => init.Prepend(last))
                .ToList();

            result[0].Should().Be(sut[2]);
            result[1].Should().Be(sut[0]);
            result[2].Should().Be(sut[1]);
        }

        [Test]
        public void InitTailMatch1ElementTest()
        {

            var sut = new[]
            {
                1,
            };

            var result = sut.Match(
                (init, last) => init.Prepend(last))
                .ToList();

            result[0].Should().Be(sut[0]);
        }
    }
}