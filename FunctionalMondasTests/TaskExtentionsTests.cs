using FluentAssertions;
using FunctionalMonads.Monads.TaskMonad;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FunctionalMondasTests
{
    [TestFixture]
    public class TaskExtentionsTests
    {
        [Test]
        public async Task MapTest()
        {
            // arrange
            var taskNumber = Task.FromResult(5);

            // act
            var result = await taskNumber.Map(i => i * 2);
            
            // assert
            result.Should().Be(10);
        }

        [Test]
        public async Task FlattenTest()
        {
            // arrange
            var nested = Task.FromResult(Task.FromResult(1));

            // act
            var result = await nested.Flatten();

            // assert
            result.Should().Be(1);
        }

        [Test]
        public async Task BindTest()
        {
            // arrange
            static Task<string> AddWorld(string a) => Task.FromResult(a + " World");

            var firstTask = Task.FromResult("Hello");

            // act
            var result = await firstTask.Bind(AddWorld);

            // assert
            result.Should().Be("Hello World");
        }
    }
}
