// Build a task scheduler
namespace TaskScheduler
{
    // Simple task priority enum
    public enum TaskPriority
    {
        Low,
        Normal,
        High
    }

    // Interface for task definition
    public interface IScheduledTask
    {
        string Name { get; }
        TaskPriority Priority { get; }
        TimeSpan Interval { get; }
        DateTime LastRun { get; }
        Task ExecuteAsync();
    }

    // A basic implementation of a scheduled task
    public class SimpleTask : IScheduledTask
    {
        private readonly Func<Task> _action;
        private DateTime _lastRun = DateTime.MinValue;

        public string Name { get; }
        public TaskPriority Priority { get; }
        public TimeSpan Interval { get; }

        public DateTime LastRun => _lastRun;

        public SimpleTask(string name, TaskPriority priority, TimeSpan interval, Func<Task> action)
        {
            Name = name;
            Priority = priority;
            Interval = interval;
            _action = action;
        }

        public async Task ExecuteAsync()
        {
            await _action();
            _lastRun = DateTime.Now;
        }
    }

    // The scheduler that students need to implement
    public class TaskScheduler
    {
        private readonly List<IScheduledTask> _tasks = new();
        private readonly object _lock = new();

        public TaskScheduler()
        {
            // Initialized in declaration
        }

        public void AddTask(IScheduledTask task)
        {
            lock (_lock)
            {
                _tasks.Add(task);
            }
        }

        public void RemoveTask(string taskName)
        {
            lock (_lock)
            {
                _tasks.RemoveAll(t => t.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public List<IScheduledTask> GetScheduledTasks()
        {
            lock (_lock)
            {
                return _tasks.ToList(); // Return a shallow copy
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                List<IScheduledTask> dueTasks;

                lock (_lock)
                {
                    // Find tasks that are due to run
                    dueTasks = _tasks
                        .Where(t => DateTime.Now - t.LastRun >= t.Interval)
                        .OrderByDescending(t => t.Priority) // High priority first
                        .ToList();
                }

                foreach (var task in dueTasks)
                {
                    if (cancellationToken.IsCancellationRequested) break;

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await task.ExecuteAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error executing task '{task.Name}': {ex.Message}");
                        }
                    });
                }

                await Task.Delay(500, cancellationToken); // Delay to prevent tight loop
            }
        }
    }


    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Task Scheduler Demo");

            // Create the scheduler
            var scheduler = new TaskScheduler();

            // Add some tasks
            scheduler.AddTask(new SimpleTask(
                "High Priority Task",
                TaskPriority.High,
                TimeSpan.FromSeconds(2),
                async () =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running high priority task");
                    await Task.Delay(500); // Simulate some work
                }
            ));

            scheduler.AddTask(new SimpleTask(
                "Normal Priority Task",
                TaskPriority.Normal,
                TimeSpan.FromSeconds(3),
                async () =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running normal priority task");
                    await Task.Delay(300); // Simulate some work
                }
            ));

            scheduler.AddTask(new SimpleTask(
                "Low Priority Task",
                TaskPriority.Low,
                TimeSpan.FromSeconds(4),
                async () =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running low priority task");
                    await Task.Delay(200); // Simulate some work
                }
            ));

            // Create a cancellation token that will cancel after 20 seconds
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

            // Or allow the user to cancel with a key press
            Console.WriteLine("Press any key to stop the scheduler...");

            // Run the scheduler in the background
            var schedulerTask = scheduler.StartAsync(cts.Token);

            // Wait for user input
            Console.ReadKey();
            cts.Cancel();

            try
            {
                await schedulerTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Scheduler stopped by cancellation.");
            }

            Console.WriteLine("Scheduler demo finished!");
        }
    }
}