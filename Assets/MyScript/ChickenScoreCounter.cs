using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScoreCounter : MonoBehaviour
{
    public GameObject chickenCounterBox;
    public AudioSource collectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chicken")
        {
            ChickenScore.currentScore += 1;
            collectSound.Play();
            Destroy(other.gameObject);
        }
    }
}
