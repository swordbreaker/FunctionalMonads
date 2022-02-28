using System;
using System.Collections.Generic;
using FluentAssertions;
using FunctionalMonads.Monads.EitherMonad;
using NUnit.Framework;

namespace FunctionalMonadsTests
{
    public class EitherTests
    {
        [Test]
        public void IsPatternMatchSomeLeft()
        {
            IEither<IEnumerable<int>, string> left = Either.Left<List<int>, string>(new List<int>() {1});

            if (left is Left<IEnumerable<int>> l)
            {
                l.Value.Should().BeAssignableTo<List<int>>();
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void IsPatternMatchSomeRight()
        {
            IEither<string, IEnumerable<int>> right = Either.Right<string, List<int>>(new List<int>() { 1 });

            if (right is Right<IEnumerable<int>> r)
            {
                r.Value.Should().BeAssignableTo<List<int>>();
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SwitchPatternMatchSomeLeft()
        {
            IEither<IEnumerable<int>, IEnumerable<int>> left = Either.Left<List<int>, List<int>>(new List<int>() { 1 });

            var result = left switch
            {
                Left<IEnumerable<int>> l => l.Value,
                Right<IEnumerable<int>> r => r.Value,
                _ => throw new ArgumentOutOfRangeException(nameof(left))
            };

            result.Should().HaveCount(1);
            result.Should().Contain(1);
        }

        [Test]
        public void SwitchPatternMatchSomeRight()
        {
            IEither<IEnumerable<int>, IEnumerable<int>> left = Either.Right<List<int>, List<int>>(new List<int>() { 2, 3 });

            var result = left switch
            {
                Left<IEnumerable<int>> l => l.Value,
                Right<IEnumerable<int>> r => r.Value,
                _ => throw new ArgumentOutOfRangeException(nameof(left))
            };

            result.Should().HaveCount(2);
            result.Should().Contain(2);
            result.Should().Contain(3);
        }
    }
}