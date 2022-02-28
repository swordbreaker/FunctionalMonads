namespace FunctionalMonadsExamples.Models.Operations
{
    internal class DecimalFormatter : IOperationFormatter<decimal>
    {
        public string Format(decimal value)
        {
            return value.ToString("F2");
        }
    }
}
