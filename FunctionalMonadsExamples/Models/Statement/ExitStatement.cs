namespace FunctionalMonadsExamples.Models.Statement
{
    internal class ExitStatement : IStatement
    {
        public string Evaluate() => "Application will be closed";
    }
}
