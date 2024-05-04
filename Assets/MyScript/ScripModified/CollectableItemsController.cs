using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public enum ItemsType { Apple, Chicken}
public class CollectableItemsController : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';
    private Dictionary<string, int> multiplierDict;
    [SerializeField] private ItemsType type;
    // Start is called before the first frame update

    public Dictionary<string, int> ScoreValueDic
    {

        get
        {
            return multiplierDict;
        }


    }
    void Start()
    {
        multiplierDict = new Dictionary<string, int>();

        string eT = type.ToString();
        string filePath = "File/" + eT + "ScoreValue";
        List<int> dM = new List<int>();

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
                    multiplierDict.Add(token[0], int.Parse(token[1], CultureInfo.InvariantCulture));
                    break;
            }
        }

    }
}
