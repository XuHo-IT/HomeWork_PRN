using ToDo;

namespace AdvanceTodo
{
    public class MainProgram
    {
        public static void Main()
        {
            Console.WriteLine("=== AdvanceTodo Test ===");

            // BorrowingHistory
            var history = new BorrowingHistory(1, "C# in Depth", DateTime.Now, null, "Xuan Hoa")
            {
                LibraryLocation = "Downtown Branch"
            };
            Console.WriteLine($"BorrowingHistory: {history}");

            //  String Extension
            string text = "Hello World";
            bool contains = text.ContainsIgnoreCase("WORLD");
            Console.WriteLine($"ContainsIgnoreCase: {contains}");

            //  LibraryItemCollection<T>
            var bookCollection = new LibraryItemCollection<Book>();
            var book = new Book(1, "C# 1", 2024, "Test1", 20, "Logic Book");
            bookCollection.Add(book);
            Console.WriteLine($"Book Count: {bookCollection.Count}");
            bookCollection.GetItem(0)?.DisplayInfo();

            //  Ref Parameter & Ref Return
            var manager = new LibraryManager();
            manager.AddItem(new Book(2, "C# 2", 2020, "Test2", 20, "Logic Book"));

            string title = "Old Title";
            manager.UpdateItemTitle(ref title, "New Title");
            Console.WriteLine($"Updated Title via ref: {title}");

            ref var itemRef = ref manager.GetItemReference(2);
            itemRef.Title = "Ref Updated Title";
            itemRef.DisplayInfo();
        }
    }
}
