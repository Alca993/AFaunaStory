using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
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

    public List<Transform> GetEnemies()
    {
        List<Transform> e = new List<Transform>();
        foreach (GameObject item in enemies)
        {
            e.Add(item.transform);
        }
        return e;
    }
    public Enemies(int c, List<GameObject> en)
    {
        count = c;
        enemies = en;
    }
}
