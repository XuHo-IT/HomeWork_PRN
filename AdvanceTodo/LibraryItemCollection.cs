using ToDo;

namespace AdvanceTodo
{
    public class LibraryItemCollection<T> where T : LibraryItem
    {
        private List<T> items = new();

        public void Add(T item)
        {
            items.Add(item);
        }

        public T? GetItem(int index)
        {
            if (index >= 0 && index < items.Count)
                return items[index];
            return null;
        }

        public int Count => items.Count;
    }

}
