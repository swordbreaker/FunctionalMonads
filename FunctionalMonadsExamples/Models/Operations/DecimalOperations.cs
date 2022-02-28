using Dawn;
using FunctionalMonadsExamples.Models.Statement;

namespace FunctionalMonadsExamples.Models.Operations
{
    internal abstract record DecimalOperation : IStatement
    {
        public abstract decimal Calculate();

        public string Evaluate()
        {
            var formatter = new DecimalFormatter();
            return "= " + formatter.Format(Calculate());
        }
    }

    internal record AddOperations(decimal A, decimal B) : DecimalOperation
    {
        public  override decimal Calculate() => A + B;
    }

    internal record SubstractOperations(decimal A, decimal B) : DecimalOperation
    {
        public override decimal Calculate() => A - B;
    }

    internal record MultiplicationOperations(decimal A, decimal B) : DecimalOperation
    {
        public override decimal Calculate() => A * B;
    }

    internal record DivideOperations() : DecimalOperation
    {
        public decimal A { get; init; }
        public decimal B { get; init; }

        public DivideOperations(decimal a, decimal b) : 
            this()
        {
            A = a;
            B = Guard.Argument(b, nameof(b)).NotZero();
        }

        public override decimal Calculate() => A / B;
    }

    internal record PowerOfOperations(double baseNumer, double exponent) : DecimalOperation
    {
        public PowerOfOperations(decimal baseNumber, decimal exponent) : 
            this((double)baseNumber, (double)exponent) { }

        public override decimal Calculate() => (decimal)Math.Pow(baseNumer, exponent);
    }

    internal record RootOperation(double baseNumer, double exponent) : DecimalOperation
    {
        public RootOperation(decimal baseNumber, decimal exponent) :
            this((double)baseNumber, (double)exponent)
        { }

        public override decimal Calculate() => (decimal)Math.Pow(baseNumer, 1 / exponent);
    }
}
