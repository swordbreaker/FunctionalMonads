using FunctionalMonadsExamples.Models.SyntaxNodes;

namespace FunctionalMonadsExamples.Models.Operations
{
    internal interface IOperation<T> : SyntaxNode
    {
        public T Evaluate();
    }
}
