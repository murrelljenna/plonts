using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public Dictionary<string, int> collection;

    public Storage()
    {
        collection = new Dictionary<string, int>();
    }

    public void add (string name)
    {
        int value = 0;
        collection.TryGetValue(name, out value);
        collection.Add(name, value + 1);
    }

    public void remove(string name)
    {
        int value = 0;
        collection.TryGetValue(name, out value);

        if (value <= 1)
        {
            collection.Remove(name);
        }
        else
        {
            collection.Add(name, value - 1);
        }
    }

    public bool hasItem()
    {
        return collection.Count > 0;
    }

    public void clear()
    {
        collection.Clear();
    }
}