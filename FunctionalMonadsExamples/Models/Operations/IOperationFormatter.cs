namespace FunctionalMonadsExamples.Models.Operations
{
    internal interface IOperationFormatter<T>
    {
        string Format(T value);
    }
}
