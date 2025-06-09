namespace ToDo
{
    public class Library
    {
        private List<LibraryItem> items = new List<LibraryItem>();

        public void AddItem(LibraryItem item)
        {
            items.Add(item);
            Console.WriteLine($"Item '{item.Title}' added to the library.");
        }

        public void SearchByTitle(string title)
        {
            var found = items.Where(i => i.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            if (found.Count > 0)
            {
                Console.WriteLine($"Found {found.Count} item(s) matching '{title}':");
                foreach (var item in found)
                {
                    item.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine($"No items found matching '{title}'.");
            }
        }

        public void DisplayAllItems()
        {
            Console.WriteLine("Library Inventory:");
            foreach (var item in items)
            {
                item.DisplayInfo();
            }
        }
    }

}
