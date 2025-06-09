using System.Diagnostics;
using System.Text;
namespace HW2
{
    // Delegate types for processing pipeline
    public delegate string DataProcessor(string input);
    public delegate void ProcessingEventHandler(string stage, string input, string output);

    public class DataProcessingPipeline
    {
        public event ProcessingEventHandler ProcessingStageCompleted;

        public static string RemoveSpaces(string input)
        {
            string output = input.Replace(" ", "");
            return output;
        }

        public static string ToUpperCase(string input)
        {
            string output = input.ToUpper();
            return output;
        }

        public static string AddTimestamp(string input)
        {
            string output = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {input}";
            return output;
        }

        public static string ReverseString(string input)
        {
            char[] arr = input.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string EncodeBase64(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string ValidateInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.");
            return input;
        }

        // Process input through the pipeline and raise events
        public string ProcessData(string input, DataProcessor pipeline)
        {
            string currentInput = input;
            string currentOutput = input;
            if (pipeline == null)
                throw new ArgumentNullException(nameof(pipeline));

            foreach (DataProcessor processor in pipeline.GetInvocationList())
            {
                string stage = processor.Method.Name;
                try
                {
                    currentOutput = processor(currentInput);
                    OnProcessingStageCompleted(stage, currentInput, currentOutput);
                    currentInput = currentOutput;
                }
                catch (Exception ex)
                {
                    OnProcessingStageCompleted(stage, currentInput, $"[ERROR] {ex.Message}");
                    throw;
                }
            }
            return currentOutput;
        }

        protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
        {
            ProcessingStageCompleted?.Invoke(stage, input, output);
        }
    }

    // Logger class to monitor processing
    public class ProcessingLogger
    {
        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            Console.WriteLine($"[LOG] Stage: {stage}, Input: \"{input}\", Output: \"{output}\"");
        }
    }

    // Performance monitor class
    public class PerformanceMonitor
    {
        private Dictionary<string, List<long>> _timings = new Dictionary<string, List<long>>();

        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            var sw = Stopwatch.StartNew();
            // Simulate processing time (for demonstration, not needed in real use)
            sw.Stop();
            if (!_timings.ContainsKey(stage))
                _timings[stage] = new List<long>();
            _timings[stage].Add(sw.ElapsedTicks);
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n[PERFORMANCE] Processing stage timings (ticks):");
            foreach (var kvp in _timings)
            {
                double avg = kvp.Value.Count > 0 ? kvp.Value.Average() : 0;
                Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} calls, Avg: {avg:F2} ticks");
            }
        }
    }

    public class DelegateChain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 2: CUSTOM DELEGATE CHAIN ===");

            DataProcessingPipeline pipeline = new DataProcessingPipeline();
            ProcessingLogger logger = new ProcessingLogger();
            PerformanceMonitor monitor = new PerformanceMonitor();

            // Subscribe to events
            pipeline.ProcessingStageCompleted += logger.OnProcessingStageCompleted;
            pipeline.ProcessingStageCompleted += monitor.OnProcessingStageCompleted;

            // Create processing chain
            DataProcessor processingChain = DataProcessingPipeline.ValidateInput;
            processingChain += DataProcessingPipeline.RemoveSpaces;
            processingChain += DataProcessingPipeline.ToUpperCase;
            processingChain += DataProcessingPipeline.AddTimestamp;

            // Test the pipeline
            string testInput = "Hello World from C#";
            Console.WriteLine($"\nInput: {testInput}");
            string result = pipeline.ProcessData(testInput, processingChain);
            Console.WriteLine($"Output: {result}");

            // Demonstrate adding more processors
            processingChain += DataProcessingPipeline.ReverseString;
            processingChain += DataProcessingPipeline.EncodeBase64;

            // Test again with extended pipeline
            string extendedInput = "Extended Pipeline Test";
            Console.WriteLine($"\nInput: {extendedInput}");
            result = pipeline.ProcessData(extendedInput, processingChain);
            Console.WriteLine($"Extended Output: {result}");

            // Demonstrate removing a processor
            processingChain -= DataProcessingPipeline.ReverseString;
            string modifiedInput = "Without Reverse";
            Console.WriteLine($"\nInput: {modifiedInput}");
            result = pipeline.ProcessData(modifiedInput, processingChain);
            Console.WriteLine($"Modified Output: {result}");

            // Display performance statistics
            monitor.DisplayStatistics();

            // Error handling test
            try
            {
                Console.WriteLine("\nTesting error handling with null input:");
                result = pipeline.ProcessData(null, processingChain); // Should handle null input
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handled: {ex.Message}");
            }

            Console.WriteLine("\nDemo complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
