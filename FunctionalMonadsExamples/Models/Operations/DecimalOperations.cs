using Dawn;

namespace FunctionalMonadsExamples.Models.Operations
{
    internal record AddOperations(decimal A, decimal B) : IOperation<decimal>
    {
        public decimal Evaluate() => A + B;
    }

    internal record SubstractOperations(decimal A, decimal B) : IOperation<decimal>
    {
        public decimal Evaluate() => A - B;
    }

    internal record MultiplicationOperations(decimal A, decimal B) : IOperation<decimal>
    {
        public decimal Evaluate() => A * B;
    }

    internal record DivideOperations() : IOperation<decimal>
    {
        public decimal A { get; init; }
        public decimal B { get; init; }

        public DivideOperations(decimal a, decimal b) : 
            this()
        {
            A = a;
            B = Guard.Argument(b, nameof(b)).NotZero();
        }

        public decimal Evaluate() => A / B;
    }

    internal record PowerOfOperations(double baseNumer, double exponent) : IOperation<decimal>
    {
        public PowerOfOperations(decimal baseNumber, decimal exponent) : 
            this((double)baseNumber, (double)exponent) { }

        public decimal Evaluate() => (decimal)Math.Pow(baseNumer, exponent);
    }

    internal record RootOperation(double baseNumer, double exponent) : IOperation<decimal>
    {
        public RootOperation(decimal baseNumber, decimal exponent) :
            this((double)baseNumber, (double)exponent)
        { }

        public decimal Evaluate() => (decimal)Math.Pow(baseNumer, 1 / exponent);
    }
}
