using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtBarHandler : MonoBehaviour
{
    private Collider coll;
    private ObjectManager om;
    private float currentValueHealtBar;
    private float startValueHealtBar;
    public AudioSource collectSound;
    public AudioSource collectSound1;
    private void Start()
    {
        coll = GetComponent<Collider>();
        om = ObjectManager.Instance();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "heart" || other.gameObject.tag == "Fruit")
        {
            Debug.Log("Healing");
            HealingItems items = other.gameObject.GetComponent<HealingItems>();
            Dictionary<string, float> healDic = items.HealDic;
            Messenger<float>.Broadcast(GameEvent.HANDLE_HEALING, healDic["playerHealAmount"]);
            startValueHealtBar = om.StartHealtBar;
            currentValueHealtBar = om.CurrentHealtBar;
            if (!Mathf.Approximately(currentValueHealtBar, startValueHealtBar))
            {
                collectSound.Play();
                Destroy(other.gameObject);
            }/*if (other.gameObject.tag == "Fruit")
            {
                Debug.Log("Healing");
                HealingItems items1 = other.gameObject.GetComponent<HealingItems>();
                Dictionary<string, float> healDic1 = items1.HealDic;
                Messenger<float>.Broadcast(GameEvent.HANDLE_HEALING, healDic1["playerHealAmount"]);
                startValueHealtBar = om.StartHealtBar;
                currentValueHealtBar = om.CurrentHealtBar;
                if (!Mathf.Approximately(currentValueHealtBar, startValueHealtBar))
                {
                    collectSound1.Play();
                    Destroy(other.gameObject);
                }
            }*/
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Trap")
        {
            Debug.Log("Environment Damage");
            EnvironmentController env = other.gameObject.GetComponent<EnvironmentController>();
            Dictionary<string, float> multDic = env.DamagesMultiplierDic;
            Messenger<float>.Broadcast(GameEvent.HANDLE_DAMAGE_ENVIRONMENT, multDic["playerDamageMultiplier"]);
        }if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision");
              EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
              Dictionary<string, float> multDic = enemy.DamagesMultiplierDic;
              Messenger<float>.Broadcast(GameEvent.HANDLE_HEALT_BAR, multDic["playerDamageMultiplier"]);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision Exit");
        }
    }

    void OnCollisionStay(Collision collision)
      {
          if ( collision.gameObject.tag=="Axe")
          {
              Debug.Log("Collision");
              EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
              Dictionary<string, float> multDic = enemy.DamagesMultiplierDic;
              Messenger<float>.Broadcast(GameEvent.HANDLE_HEALT_BAR, multDic["playerDamageMultiplier"]);
          }
      }
   
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            Debug.Log("Collision Exit");
        }
    }
}
