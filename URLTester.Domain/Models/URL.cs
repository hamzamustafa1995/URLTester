using URLTester.Domain.Helpers;

namespace URLTester.Domain.Models;

public class URL
{
    public int Id { get; }
    public required string Original { get; set; }
    public required string Shortened { get; set; }
    public DateTime CreatedAt { get; } = DateTime.Now;
    public int ClickCount { get; private set; } = 0;

    public static URL Create(string original,bool generateShortened)
    {
        return new URL
        {
            Original = original,
            Shortened = MethodHelper.GenerateShortened(generateShortened)
        };
    }

    public void Visit()
    {
        ClickCount++;
    }
}
