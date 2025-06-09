namespace AdvanceTodo
{
    public record BorrowingHistory(int ItemId, string Title, DateTime BorrowDate, DateTime? ReturnDate, string BorrowerName)
    {
        public string LibraryLocation { get; init; } = "Main Library";
    }

}
