using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnWater : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = respawnPosition.transform.position;
            Physics.SyncTransforms();
        }
    }
}
