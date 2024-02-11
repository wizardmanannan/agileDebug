using System;
using System.Collections.Generic;

public class DebuggableList<T> : List<T> // Inheriting from List<T> is not possible as it's sealed; this is for demonstration.
{
    private List<T> _list = new List<T>();

    // Override the indexer
    public new T this[int index]
    {
        get
        {
            // Add your custom logic here
            return _list[index];
        }
        set
        {
            // Add your custom logic here
            _list[index] = value;
        }
    }

    // Add methods to manipulate the list as needed
    public void Add(T item)
    {
        _list.Add(item);
    }

    public T RemoveAt(int index)
    {
        T item = _list[index];
        _list.RemoveAt(index);
        return item;
    }

    // Implement other methods and properties of List<T> as needed
}