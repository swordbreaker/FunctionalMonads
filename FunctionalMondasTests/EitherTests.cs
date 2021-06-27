using FunctionalMonads.Monads.EitherMonad;
using NUnit.Framework;

namespace FunctionalMonadsTests
{
    public class EitherTests
    {
        [Test]
        public void Test()
        {
            var either1 = Either.Left<string, double>(TestHelper.RandomString());

            var either2 = Either.Left<string, int>(TestHelper.RandomString());
        }
    }
}