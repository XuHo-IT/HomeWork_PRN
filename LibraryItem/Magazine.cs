namespace ToDo
{
    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public string Publisher { get; set; }

        public Magazine(int id, string title, int publicationYear, int issueNumber, string publisher)
            : base(id, title, publicationYear)
        {
            IssueNumber = issueNumber;
            Publisher = publisher;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Magazine - ID: {Id}, Title: {Title}, Issue: {IssueNumber}, Publisher: {Publisher}, Year: {PublicationYear}");
        }
    }

}
