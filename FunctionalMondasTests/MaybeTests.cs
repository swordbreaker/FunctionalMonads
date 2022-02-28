using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FunctionalMonads.Monads;
using FunctionalMonads.Monads.MaybeMonad;
using NUnit.Framework;
using static FunctionalMonads.Monads.MaybeMonad.Maybe;
using static FunctionalMonadsTests.TestHelper;
using None = FunctionalMonads.Monads.MaybeMonad.None;

namespace FunctionalMonadsTests
{
    public class Tests
    {

        [Test]
        public void MapSomeTest()
        {
            // arrange
            var some = RandomSome(RandomInt, out var number);

            // act
            var mapSome = some.Map(i => i / 2.0);

            // assert
            mapSome.IsSome.Should().BeTrue();
            var doubleSome = mapSome.Should().BeAssignableTo<MaybeSome<double>>().Subject;
            doubleSome.Value.Should().Be(number / 2.0);
        }

        [Test]
        public void MapNoneTest()
        {
            // arrange
            var none = None<int>();

            // act
            var mapNone = none.Map(i => i / 2.0);

            // assert
            mapNone.IsNone.Should().BeTrue();
            mapNone.Should().BeAssignableTo<MaybeNone<double>>();
        }

        [Test]
        public void IsPatternMatchingSomeTest()
        {
            // arrange
            IMaybe<IEnumerable<int>> some = Some(new List<int>());

            // assert
            if (some is Some<IEnumerable<int>> s)
            {
                s.Value.Should().BeAssignableTo<List<int>>();
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ISPatternMatchingNoneTest()
        {
            // arrange
            IMaybe<IEnumerable<int>> none = None<List<int>>();

            // assert
            if (none is None && none is None<IEnumerable<int>>)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SwitchPatternMatchingSomeTest()
        {
            // arrange
            IMaybe<IEnumerable<int>> some = Some(new List<int>(){ 1 });

            // act
            var result = some switch
            {
                Some<IEnumerable<int>> x =>
                    x.Value,
                None _ =>
                    Enumerable.Empty<int>(),
                _ => throw new System.NotImplementedException(),
            };

            // assert
            result.Should().HaveCount(1);
        }

        [Test]
        public void SwitchPatternMatchingNoneTest()
        {
            // arrange
            var none = None<List<int>>();

            // act
            var result = none switch
            {
                Some<IEnumerable<int>> x =>
                    x.Value,
                None _ =>
                    Enumerable.Empty<int>(),
                _ => throw new System.NotImplementedException(),
            };

            // assert
            result.Should().HaveCount(0);
        }

        [Test]
        public void BindSomeTest()
        {
            // arrange
            var prefix = RandomString();
            var some = RandomSome(RandomString, out var value);

            // act
            var result = some.Bind(s => Maybe.Some(prefix + s));

            // assert
            result.IsSome.Should().BeTrue();
            var someString = result.Should().BeAssignableTo<MaybeSome<string>>().Subject;
            someString.Value.Should().Be(prefix + value);
        }

        [Test]
        public void BindNoneTest()
        {
            // arrange
            var none = None<string>();

            // act
            var result = none.Bind(s => Some(RandomString() + s));

            // assert
            result.IsSome.Should().BeFalse();
            result.Should().BeAssignableTo<MaybeNone<string>>();
        }

        [Test]
        public void SelectManySomeTest()
        {
            // arrange
            var some1 = RandomSome(RandomInt, out var intVal);
            var some2 = RandomSome(RandomString, out var stringVal);

            // act
            var result = (
                from i in some1
                from s in some2
                select s + i);

            // assert
            result.IsSome.Should().BeTrue();
            var someResult = result.Should().BeAssignableTo<MaybeSome<string>>().Subject;
            someResult.Value.Should().Be(stringVal + intVal);
        }

        [Test]
        public void SelectManyNoneTest()
        {
            // arrange
            var some1 = RandomSome(RandomInt, out var intVal);
            var some2 = RandomSome(RandomString, out var stringVal);
            var none = None<int>();

            // act
            var result = (
                from i in some1
                from s in some2
                from x in none
                select s + i + x);

            // assert
            result.IsSome.Should().BeFalse();
            result.Should().BeAssignableTo<MaybeNone<string>>();
        }

        [Test]
        public void MatchSomeTest()
        {
            // arrange
            var some = RandomSome(RandomString, out var s);

            // act
            var result = some.Match(x => x, () => "MaybeNone");

            // assert
            result.Should().Be(s);
        }

        [Test]
        public void SomeIfSomeTest()
        {
            // arrange
            var x = RandomInt();
            var y = 0;
            var some = RandomSome(RandomInt, out var intVal);

            // act
            var unit = some.IfSome(i => y = x + i);

            // assert
            unit.Should().BeEquivalentTo(new Unit());
            y.Should().Be(x + intVal);
        }

        [Test]
        public void NoneIfSomeTest()
        {
            // arrange
            var x = RandomInt();
            var y = 0;
            var none = None<int>();

            // act
            var unit = none.IfSome(i => y = x + i);

            // assert
            unit.Should().BeEquivalentTo(new Unit());
            y.Should().Be(0);
        }

        [Test]
        public void SomeIfNoneTest()
        {
            // arrange
            var x = RandomInt();
            var y = 0;
            var some = RandomSome(RandomInt, out var intVal);

            // act
            var unit = some.IfNone(() => y = x);

            // assert
            unit.Should().BeEquivalentTo(new Unit());
            y.Should().Be(0);
        }

        [Test]
        public void NoneIfNoneTest()
        {
            // arrange
            var x = RandomInt();
            var y = 0;
            var none = None<int>();

            // act
            var unit = none.IfNone(() => y = x);

            // assert
            unit.Should().BeEquivalentTo(new Unit());
            y.Should().Be(x);
        }
    }
}