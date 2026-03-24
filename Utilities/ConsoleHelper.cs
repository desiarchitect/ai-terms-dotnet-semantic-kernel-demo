static class ConsoleHelper
{
    public static void PrintHeader(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', text.Length + 4));
        Console.WriteLine($"  {text}");
        Console.WriteLine(new string('=', text.Length + 4));
        Console.ResetColor();
    }

    public static void PrintSection(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n{text}");
        Console.WriteLine(new string('-', text.Length));
        Console.ResetColor();
    }

    public static void WriteKeyValue(string key, string value)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{key}: ");
        Console.ResetColor();
        Console.WriteLine(value);
    }

    public static void WriteLineAccent(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void Pause()
    {
        Console.WriteLine("Press Enter to continue to the next demo...");
        Console.ReadLine();
    }
}
