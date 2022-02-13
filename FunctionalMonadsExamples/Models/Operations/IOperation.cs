using FunctionalMonadsExamples.Models.Statement;

namespace FunctionalMonadsExamples.Models.Operations
{
    internal interface IOperation<T> : IStatement
    {
        string Calculate(IOperationFormatter<T> formatter);
    }
}
