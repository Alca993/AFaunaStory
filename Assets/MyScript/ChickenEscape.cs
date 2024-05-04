using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenEscape : MonoBehaviour
{
    public NavMeshAgent chicken;
    // private Transform player;
    private Transform target;
    private GameObject player;
    private int multiplier = 1; 
    float range = 8.0f;
    // Start is called before the first frame update
    void Start()
    {
        chicken = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 runTo = transform.position + ((transform.position - target.position) * multiplier);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < range) chicken.SetDestination(runTo);
    }
}
