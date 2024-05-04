using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEditorInternal;
using System.Linq;

public class Spawner : MonoBehaviour
{
    private GameControl gameControl;
    private const char NEW_LINE = '\n';
    private const char SEMICOLON = ';';

    private string filePath = "File/Level1SpawnPos";
    private string itemPath = "ItemsPrefab/";

    [SerializeField] protected Transform[] spawnPoints;
    
    private Dictionary<string, Items> keys = new Dictionary<string, Items>();
    private List<Transform> items = new List<Transform>();
    private int numSpawnPoints;
    private int spawnIndex = -1;

    void Start()
    {
        gameControl = GameControl.GetInstance();

        int currentLevel = gameControl.currentLevel;

        filePath = "File/Level" + currentLevel + "SpawnPos";
        Debug.Log("CURRENT LEVEL: " + filePath);
        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        numSpawnPoints = int.Parse(lines[0].Split(SEMICOLON)[1], CultureInfo.InvariantCulture);

        for (int i = 2; i < lines.Length; i++)
        {
            List<GameObject> itemsPrefabs = new List<GameObject>();
            string itemPrefab = "";

            string[] row = lines[i].Split(SEMICOLON);

            string SpID = row[0];
            int itemsNum = int.Parse(row[1], CultureInfo.InvariantCulture);
            Debug.Log(SpID);

            for (int j = 2; j < row.Length; j++)
            {
                if (row[j].Equals("A"))
                    itemPrefab = itemPath + "AppleItem";
                else if (row[j].Equals("C"))
                    itemPrefab = itemPath + "Chicken";
                else if (row[j].Equals("H"))
                    itemPrefab = itemPath + "MyHeartPrefab";
                else if (row[j].Equals("P"))
                    itemPrefab = itemPath + "Pear";
                else if (!row[j].Equals("A") && !row[j].Equals("C") && !row[j].Equals("H") && !row[j].Equals("P"))
                {
                    continue;
                }
                itemsPrefabs.Add(Resources.Load<GameObject>(itemPrefab));
                itemPrefab = "";
            }
            Items item = new Items(itemsNum, itemsPrefabs);
            keys.Add(SpID, item);
        }
        Spawn();
    }

    private void Spawn()
    {
        Items[] itemSpawnPoints = new Items[numSpawnPoints];
        int spCount = 0;

        for (int i = 0; i < numSpawnPoints; i++)
        {
            spawnIndex++;
            itemSpawnPoints[i] = keys.ElementAt(spawnIndex).Value;
            if (itemSpawnPoints[i].GetItems().Count > spCount)
                spCount = itemSpawnPoints[i].GetItems().Count;
        }
        for (int i = 0; i <= spCount; i++)
        {
            for (int j = 0; j < numSpawnPoints; j++)
            {
                if (i >= itemSpawnPoints[j].GetItems().Count)
                {
                    continue;
                }
                else
                {
                    Transform item = itemSpawnPoints[j].GetItems()[i];
                    SpawnItem(spawnPoints[j], item);

                }
            }
        }
     }

    private void SpawnItem(Transform point, Transform item)
    {
            items.Add(Instantiate(item, point.position, point.rotation));
    }
}