namespace FunctionalMonadsExamples.Models.Statement
{
    internal class TimeStatement : IStatement
    {
        public string Evaluate() => DateTime.Now.ToString("f");
    }
}
