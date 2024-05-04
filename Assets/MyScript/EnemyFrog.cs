using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Globalization;

public class EnemyFrog : MonoBehaviour
{

    public NavMeshAgent enemy;
    private Transform target;
    private GameObject player;
    private int multiplier;
    float range;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("AttackPoint");
        target = player.transform;

        TextAsset data = Resources.Load<TextAsset>("File/enemyFrogFeatures");
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "multiplier":
                    multiplier = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "range":
                    range = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("damageTimeout: " + range);
                    break;
                default:

                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveToPlayer();
    }
    void moveToPlayer()
    {
        Vector3 runTo = transform.position + (((target.position) - transform.position) * multiplier);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < range) enemy.SetDestination(runTo);
    }
}
