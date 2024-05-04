using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCounter : MonoBehaviour
{
    private GameObject appleCounter;
    private GameObject chickenCounter;
    public AudioSource collectSound;
    public AudioSource collectSound1;
    private bool hasCollectApple;
    private bool hasCollectChicken;

    public bool HasCollectApple
    {
        get
        {
            return hasCollectApple;
        }
    }
    public bool HasCollectChicken
    {
        get
        {
            return hasCollectChicken;
        }
    }
    private void Awake()
    {
        appleCounter = GameObject.FindGameObjectWithTag("AppleCounter");
        chickenCounter = GameObject.FindGameObjectWithTag("ChickenCounter");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chicken")
        {
            CollectableItemsController itemsC = other.gameObject.GetComponent<CollectableItemsController>();
            Dictionary<string, int> scoreDic = itemsC.ScoreValueDic; 
            Messenger<int>.Broadcast(GameEvent.HANDLE_COLLECT_CHICKEN, scoreDic["scoreValue"]);
            collectSound.Play();
                Destroy(other.gameObject);
                hasCollectChicken = true;
        }else if(other.tag == "Apple")
        {
            CollectableItemsController itemsC = other.gameObject.GetComponent<CollectableItemsController>();
            Dictionary<string, int> scoreDic = itemsC.ScoreValueDic;
            Messenger<int>.Broadcast(GameEvent.HANDLE_COLLECT_APPLE, scoreDic["scoreValue"]);
            collectSound1.Play();
                Destroy(other.gameObject);
                hasCollectApple = true;
        }
    }
}
