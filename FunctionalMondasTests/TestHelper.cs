using System;
using System.Configuration.Internal;
using FunctionalMonads.Monads.MaybeMonad;

namespace FunctionalMonadsTests
{
    public static class TestHelper
    {
        private static Random rnd = new Random();

        public static string RandomString() =>
            Guid.NewGuid().ToString();

        public static int RandomInt(int min, int max = int.MaxValue) => 
            rnd.Next(min, max);

        public static int RandomInt() =>
            rnd.Next(int.MinValue, int.MaxValue);

        public static IMaybe<T> RandomSome<T>(Func<T> randomInnerFunc, out T randomInnerValue)
        {
            randomInnerValue = randomInnerFunc();
            return Maybe.Some(randomInnerValue);
        }
    }
}