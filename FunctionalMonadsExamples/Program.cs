using FunctionalMonadsExamples.Parsers;

var parser = new CalculationParser();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Add a calulation:");
do
{
    var line = Console.ReadLine();

    if (line != null)
    {
        parser.Parse(line)
        .MapLeft(operation => operation.Evaluate())
        .MapLeft(d => d.ToString("F2"))
        .Do(
            x => Console.WriteLine($"= {x}"),
            f => Console.WriteLine(f.Message));
    }
} while (true);