namespace ToDo
{
    public class DVD : LibraryItem, IBorrowable
    {
        public string Director { get; set; }
        public int Runtime { get; set; } // in minutes
        public string AgeRating { get; set; }

        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable { get; set; } = true;

        public DVD(int id, string title, int publicationYear, string director, int runtime, string ageRating)
            : base(id, title, publicationYear)
        {
            Director = director;
            Runtime = runtime;
            AgeRating = ageRating;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"DVD - ID: {Id}, Title: {Title}, Director: {Director}, Runtime: {Runtime} mins, AgeRating: {AgeRating}, Year: {PublicationYear}");
        }

        public override decimal CalculateLateReturnFee(int daysLate)
        {
            return daysLate * 1.00m;
        }

        public void Borrow()
        {
            if (IsAvailable)
            {
                BorrowDate = DateTime.Now;
                IsAvailable = false;
                Console.WriteLine($"DVD '{Title}' borrowed on {BorrowDate}");
            }
            else
            {
                Console.WriteLine($"DVD '{Title}' is not available.");
            }
        }

        public void Return()
        {
            if (!IsAvailable)
            {
                ReturnDate = DateTime.Now;
                IsAvailable = true;
                Console.WriteLine($"DVD '{Title}' returned on {ReturnDate}");
            }
            else
            {
                Console.WriteLine($"DVD '{Title}' was not borrowed.");
            }
        }
    }

}
