using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public enum HealingCollectionType { Heart, Fruits}
public class HealingItems : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';
    private Dictionary<string, float> amountDict;
    [SerializeField] private HealingCollectionType type;

    public Dictionary<string, float> HealDic
    {

        get
        {
            return amountDict;
        }
    }
    void Start()
    {
        amountDict = new Dictionary<string, float>();

        string eT = type.ToString();
        string filePath = "File/" + eT + "HealAmounts";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                default:
                    Debug.Log(token[0]);
                    amountDict.Add(token[0], float.Parse(token[1], CultureInfo.InvariantCulture));
                    break;
            }
        }
    }
}
