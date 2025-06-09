namespace HW3
{
    // Data models
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; } // 0-4.0 scale
        public string Semester { get; set; }
        public string Instructor { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class StudentStatistics
    {
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
    }

    // Extension methods
    public static class StudentExtensions
    {
        public static IEnumerable<Student> FilterByAgeRange(this IEnumerable<Student> students, int minAge, int maxAge)
        {
            return students.Where(s => s.Age >= minAge && s.Age <= maxAge);
        }

        public static Dictionary<string, double> AverageGPAByMajor(this IEnumerable<Student> students)
        {
            return students
                .GroupBy(s => s.Major)
                .ToDictionary(g => g.Key, g => g.Average(s => s.GPA));
        }

        public static string ToGradeReport(this Student student)
        {
            var courses = string.Join(", ", student.Courses.Select(c => $"{c.Code}:{c.Grade:F1}"));
            return $"{student.Name} ({student.Major}) - GPA: {student.GPA:F2} | Courses: {courses}";
        }

        public static StudentStatistics CalculateStatistics(this IEnumerable<Student> students)
        {
            var gpas = students.Select(s => s.GPA).OrderBy(g => g).ToList();
            double mean = gpas.Average();
            double median = gpas.Count % 2 == 1
                ? gpas[gpas.Count / 2]
                : (gpas[gpas.Count / 2 - 1] + gpas[gpas.Count / 2]) / 2.0;
            double stddev = Math.Sqrt(gpas.Average(v => Math.Pow(v - mean, 2)));
            return new StudentStatistics { Mean = mean, Median = median, StandardDeviation = stddev };
        }
    }

    // LINQ Processor
    public class LinqDataProcessor
    {
        private List<Student> _students;

        public LinqDataProcessor()
        {
            _students = GenerateSampleData();
        }

        public void BasicQueries()
        {
            Console.WriteLine("=== BASIC LINQ QUERIES ===");

            var highGPA = _students.Where(s => s.GPA > 3.5);
            Console.WriteLine("Students with GPA > 3.5:");
            foreach (var s in highGPA)
                Console.WriteLine($"  {s.Name} ({s.Major}) - GPA: {s.GPA}");

            var byMajor = _students.GroupBy(s => s.Major);
            Console.WriteLine("\nStudents grouped by major:");
            foreach (var group in byMajor)
            {
                Console.WriteLine($"Major: {group.Key}");
                foreach (var s in group)
                    Console.WriteLine($"  {s.Name}");
            }

            var avgGPAByMajor = _students
                .GroupBy(s => s.Major)
                .Select(g => new { Major = g.Key, AvgGPA = g.Average(s => s.GPA) });
            Console.WriteLine("\nAverage GPA per major:");
            foreach (var item in avgGPAByMajor)
                Console.WriteLine($"  {item.Major}: {item.AvgGPA:F2}");

            var cs101 = _students.Where(s => s.Courses.Any(c => c.Code == "CS101"));
            Console.WriteLine("\nStudents enrolled in CS101:");
            foreach (var s in cs101)
                Console.WriteLine($"  {s.Name}");

            var sorted = _students.OrderBy(s => s.EnrollmentDate);
            Console.WriteLine("\nStudents sorted by enrollment date:");
            foreach (var s in sorted)
                Console.WriteLine($"  {s.Name}: {s.EnrollmentDate:yyyy-MM-dd}");
        }

        public void CustomExtensionMethods()
        {
            Console.WriteLine("\n=== CUSTOM EXTENSION METHODS ===");

            var avgGPA = _students.AverageGPAByMajor();
            Console.WriteLine("Average GPA by major (extension):");
            foreach (var kv in avgGPA)
                Console.WriteLine($"  {kv.Key}: {kv.Value:F2}");

            var ageRange = _students.FilterByAgeRange(20, 22);
            Console.WriteLine("\nStudents age 20-22:");
            foreach (var s in ageRange)
                Console.WriteLine($"  {s.Name} ({s.Age})");

            Console.WriteLine("\nGrade reports:");
            foreach (var s in _students)
                Console.WriteLine(s.ToGradeReport());

            var stats = _students.CalculateStatistics();
            Console.WriteLine("\nStudent GPA statistics:");
            Console.WriteLine($"  Mean: {stats.Mean:F2}, Median: {stats.Median:F2}, StdDev: {stats.StandardDeviation:F2}");
        }

        // Placeholder methods for advanced sections
        public void DynamicQueries() => Console.WriteLine("=== DYNAMIC QUERIES ===\n(To be implemented)");
        public void StatisticalAnalysis() => Console.WriteLine("=== STATISTICAL ANALYSIS ===\n(To be implemented)");
        public void PivotOperations() => Console.WriteLine("=== PIVOT OPERATIONS ===\n(To be implemented)");

        private List<Student> GenerateSampleData()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1, Name = "Alice Johnson", Age = 20, Major = "Computer Science",
                    GPA = 3.8, EnrollmentDate = new DateTime(2022, 9, 1),
                    Email = "alice.j@university.edu",
                    Address = new Address { City = "Seattle", State = "WA", ZipCode = "98101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS101", Name = "Intro to Programming", Credits = 3, Grade = 3.7, Semester = "Fall 2022", Instructor = "Dr. Smith" },
                        new Course { Code = "MATH201", Name = "Calculus II", Credits = 4, Grade = 3.9, Semester = "Fall 2022", Instructor = "Prof. Johnson" }
                    }
                },
                new Student
                {
                    Id = 2, Name = "Bob Wilson", Age = 22, Major = "Mathematics",
                    GPA = 3.2, EnrollmentDate = new DateTime(2021, 9, 1),
                    Email = "bob.w@university.edu",
                    Address = new Address { City = "Portland", State = "OR", ZipCode = "97201" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "MATH301", Name = "Linear Algebra", Credits = 3, Grade = 3.3, Semester = "Spring 2023", Instructor = "Dr. Brown" },
                        new Course { Code = "STAT101", Name = "Statistics", Credits = 3, Grade = 3.1, Semester = "Spring 2023", Instructor = "Prof. Davis" }
                    }
                },
                new Student
                {
                    Id = 3, Name = "Carol Davis", Age = 19, Major = "Computer Science",
                    GPA = 3.9, EnrollmentDate = new DateTime(2023, 9, 1),
                    Email = "carol.d@university.edu",
                    Address = new Address { City = "San Francisco", State = "CA", ZipCode = "94101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS102", Name = "Data Structures", Credits = 4, Grade = 4.0, Semester = "Fall 2023", Instructor = "Dr. Smith" },
                        new Course { Code = "CS201", Name = "Algorithms", Credits = 3, Grade = 3.8, Semester = "Fall 2023", Instructor = "Prof. Lee" }
                    }
                }
            };
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var processor = new LinqDataProcessor();

            processor.BasicQueries();
            processor.CustomExtensionMethods();

            // Uncomment these when implemented
            // processor.DynamicQueries();
            // processor.StatisticalAnalysis();
            // processor.PivotOperations();
        }
    }
}
