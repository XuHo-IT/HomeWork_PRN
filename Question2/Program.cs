// A utility to analyze text files and provide statistics
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("File Analyzer - .NET Core");
            Console.WriteLine("This tool analyzes text files and provides statistics.");

            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command-line argument.");
                Console.WriteLine("Example: dotnet run myfile.txt");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' does not exist.");
                return;
            }

            try
            {
                Console.WriteLine($"Analyzing file: {filePath}");

                // Read the file content
                string content = File.ReadAllText(filePath);

                // TODO: Implement analysis functionality
                // 1. Count words
                // 2. Count characters (with and without whitespace)
                // 3. Count sentences
                // 4. Identify most common words
                // 5. Average word length

                // Example implementation for counting lines:
                int lineCount = File.ReadAllLines(filePath).Length;
                Console.WriteLine($"Number of lines: {lineCount}");
                // 1. Count words
                var words = Regex.Matches(content, @"\b\w+\b")
                                      .Cast<Match>()
                                      .Select(m => m.Value.ToLower())
                                      .ToList();
                int wordCount = words.Count;
                Console.WriteLine($"Number of words: {wordCount}");
                // 2. Count characters
                int charWithSpaces = content.Length;
                int charWithoutSpaces = content.Count(c => !char.IsWhiteSpace(c));
                Console.WriteLine($"Characters (with spaces): {charWithSpaces}");
                Console.WriteLine($"Characters (without spaces): {charWithoutSpaces}");
                // 3. Count sentences
                int sentenceCount = Regex.Matches(content, @"[.!?]+").Count;
                Console.WriteLine($"Number of sentences: {sentenceCount}");
                // 4. Identify most common words
                var mostCommon = words.GroupBy(w => w)
                            .OrderByDescending(g => g.Count())
                            .Take(5);
                Console.WriteLine("Most common words:");
                foreach (var wordGroup in mostCommon)
                {
                    Console.WriteLine($"- {wordGroup.Key}: {wordGroup.Count()} times");
                }
                // 5. Average word length
                double avgWordLength = wordCount > 0 ? words.Average(w => w.Length) : 0;
                Console.WriteLine($"Average word length: {avgWordLength:F2}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file analysis: {ex.Message}");
            }
        }
    }
}