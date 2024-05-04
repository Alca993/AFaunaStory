using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();
    private int count;

    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
        }
    }

    public List<Transform> GetItems()
    {
        List<Transform> e = new List<Transform>();
        foreach(GameObject item in items)
        {
            e.Add(item.transform);
        }
        return e;
    }
    public Items(int c,List<GameObject> it)
    {
        count=c;
        items = it;
    }
}
