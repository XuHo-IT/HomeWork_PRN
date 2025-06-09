namespace ToDo
{
    public class Book : LibraryItem, IBorrowable
    {
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }

        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book(int id, string title, int publicationYear, string author, int pages, string genre)
            : base(id, title, publicationYear)
        {
            Author = author;
            Pages = pages;
            Genre = genre;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Book - ID: {Id}, Title: {Title}, Author: {Author}, Pages: {Pages}, Genre: {Genre}, Year: {PublicationYear}");
        }

        public override decimal CalculateLateReturnFee(int daysLate)
        {
            return daysLate * 0.75m;
        }

        public void Borrow()
        {
            if (IsAvailable)
            {
                BorrowDate = DateTime.Now;
                IsAvailable = false;
                Console.WriteLine($"Book '{Title}' borrowed on {BorrowDate}");
            }
            else
            {
                Console.WriteLine($"Book '{Title}' is not available.");
            }
        }

        public void Return()
        {
            if (!IsAvailable)
            {
                ReturnDate = DateTime.Now;
                IsAvailable = true;
                Console.WriteLine($"Book '{Title}' returned on {ReturnDate}");
            }
            else
            {
                Console.WriteLine($"Book '{Title}' was not borrowed.");
            }
        }
    }

}
