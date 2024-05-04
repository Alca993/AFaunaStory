using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public enum EnvironmentType { NeedleTraps, Axe, Water}
public class EnvironmentController : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';
    private Dictionary<string, float> multiplierDict;
    [SerializeField] private EnvironmentType type;

    public Dictionary<string, float> DamagesMultiplierDic
    {

        get
        {
            return multiplierDict;
        }


    }
    void Start()
    {
        multiplierDict = new Dictionary<string, float>();

        string eT = type.ToString();
        string filePath = "File/" + eT + "DamageMultipliers";

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
                    multiplierDict.Add(token[0], float.Parse(token[1], CultureInfo.InvariantCulture));
                    break;
            }
        }

    }
}
