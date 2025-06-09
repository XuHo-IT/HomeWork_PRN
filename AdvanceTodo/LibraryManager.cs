using ToDo;

namespace AdvanceTodo
{
    public class LibraryManager
    {
        private LibraryItem[] items = new LibraryItem[10];
        private int count = 0;

        public void AddItem(LibraryItem item)
        {
            if (count < items.Length)
            {
                items[count++] = item;
            }
            else
            {
                throw new InvalidOperationException("Library is full.");
            }
        }

        public void UpdateItemTitle(ref string title, string newTitle)
        {
            title = newTitle;
        }

        public ref LibraryItem GetItemReference(int id)
        {
            for (int i = 0; i < count; i++)
            {
                if (items[i] != null && items[i].Id == id)
                    return ref items[i];
            }
            throw new InvalidOperationException("Item not found.");
        }
    }
}

