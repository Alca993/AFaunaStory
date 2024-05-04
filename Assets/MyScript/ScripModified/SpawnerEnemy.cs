using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEditorInternal;
using System.Linq;

public class SpawnerEnemy : MonoBehaviour
{
    private GameControl gameControl;
    private const char NEW_LINE = '\n';
    private const char SEMICOLON = ';';

    private string filePath = "File/Level1EnemiesSpawnPos";
    private string enemyPath = "EnemyPrefab/";

    [SerializeField] protected Transform[] spawnPoints;

    private Dictionary<string, Enemies> keys = new Dictionary<string, Enemies>();
    private List<Transform> enemies = new List<Transform>();
    private int numSpawnPoints;
    private int spawnIndex = -1;
   
    void Start()
    {
        gameControl = GameControl.GetInstance();

        int currentLevel = gameControl.currentLevel;

        filePath = "File/Level" + currentLevel + "EnemiesSpawnPos";
        Debug.Log("CURRENT LEVEL: " + filePath);
        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        numSpawnPoints = int.Parse(lines[0].Split(SEMICOLON)[1], CultureInfo.InvariantCulture);

        for (int i = 2; i < lines.Length; i++)
        {
            List<GameObject> enemiesPrefabs = new List<GameObject>();
            string enemyPrefab = "";

            string[] row = lines[i].Split(SEMICOLON);

            string SpID = row[0];
            int enemiesNum = int.Parse(row[1], CultureInfo.InvariantCulture);
            Debug.Log(SpID);

            for (int j = 2; j < row.Length; j++)
            {
                if (row[j].Equals("C"))
                    enemyPrefab = enemyPath + "Farmer";
                if (row[j].Equals("F"))
                    enemyPrefab = enemyPath + "Frog";
                if (row[j].Equals("S"))
                    enemyPrefab = enemyPath + "Spider";
                if (row[j].Equals("H"))
                    enemyPrefab = enemyPath + "Hunter";
                else if (!row[j].Equals("C") && !row[j].Equals("F") && !row[j].Equals("S") && !row[j].Equals("H"))
                {
                    continue;
                }
                enemiesPrefabs.Add(Resources.Load<GameObject>(enemyPrefab));
                enemyPrefab = "";
            }
            Enemies enemy = new Enemies(enemiesNum, enemiesPrefabs);
            keys.Add(SpID,enemy);
        }
        Spawn();
    }

    private void Spawn()
    {
        Enemies[] enemySpawnPoints = new Enemies[numSpawnPoints];
        int spCount = 0;

        for (int i = 0; i < numSpawnPoints; i++)
        {
            spawnIndex++;
            enemySpawnPoints[i] = keys.ElementAt(spawnIndex).Value;
            if (enemySpawnPoints[i].GetEnemies().Count > spCount)
                spCount = enemySpawnPoints[i].GetEnemies().Count;
        }
        for (int i = 0; i <= spCount; i++)
        {
            for (int j = 0; j < numSpawnPoints; j++)
            {
                if (i >= enemySpawnPoints[j].GetEnemies().Count)
                {
                    continue;
                }
                else
                {
                    Transform enemy = enemySpawnPoints[j].GetEnemies()[i]; 
                    SpawnEnemies(spawnPoints[j], enemy);

                }
            }
        }
    }

    private void SpawnEnemies(Transform point, Transform enemy)
    {
        enemies.Add(Instantiate(enemy, point.position, point.rotation));
    }
}
