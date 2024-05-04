using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectApple : MonoBehaviour
{
    public GameObject ScoreBox;
    public AudioSource collectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Apple")
        {
            Score.currentScore += 1;
            collectSound.Play();
            Destroy(other.gameObject);
        }
    }
}
