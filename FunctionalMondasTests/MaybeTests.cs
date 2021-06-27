using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Formatting;
using FunctionalMonads.Monads;
using FunctionalMonads.Monads.MaybeMonad;
using NUnit.Framework;
using static FunctionalMonads.Monads.MaybeMonad.Maybe;
using static FunctionalMonadsTests.TestHelper;

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
            var doubleSome = mapSome.Should().BeAssignableTo<Some<double>>().Subject;
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
            mapNone.Should().BeAssignableTo<None<double>>();
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
            var someString = result.Should().BeAssignableTo<Some<string>>().Subject;
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
            result.Should().BeAssignableTo<None<string>>();
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
            var someResult = result.Should().BeAssignableTo<Some<string>>().Subject;
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
            result.Should().BeAssignableTo<None<string>>();
        }

        [Test]
        public void MatchSomeTest()
        {
            // arrange
            var some = RandomSome(RandomString, out var s);

            // act
            var result = some.Match(x => x, () => "None");

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