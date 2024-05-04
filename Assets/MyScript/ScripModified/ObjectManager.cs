using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Threading;
using DamagePackage;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;
    private float startHealtBar;
    private float currentHealtBar;
    private int startApples;
    private int currentApples;
    private int startChickens;
    private int currentChickens;
    private GameControl gameControl;
    private int countDown;
    private int powerUp;
    private int powerUpCountDown;
   

    public float StartHealtBar
    {
        get
        {
            return startHealtBar;
        }
    }

    public float CurrentHealtBar
    {
        get
        {
            return currentHealtBar;
        }
    }
    public int CurrentApples
    {
        get
        {
            return currentApples;
        }
    }

    public int CurrentChickens
    {
        get
        {
            return currentChickens;
        }
    }
    public int PowerUp
    {
        get
        {
            return powerUp;
        }
    }
    public int CountDown
    {
        get
        {
            return countDown;
        }
    }

    public static ObjectManager Instance()
    {

        if (_instance == null)
            _instance = FindObjectOfType<ObjectManager>();
        return _instance;
    }
    private void Awake()
    {
        Messenger<Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Messenger<int>.AddListener(GameEvent.HANDLE_COLLECT_APPLE, OnHandleCollectApple);
        Messenger<int>.AddListener(GameEvent.HANDLE_COLLECT_CHICKEN, OnHandleCollectChicken);
        Messenger<float>.AddListener(GameEvent.HANDLE_HEALT_BAR, OnHandleHealtBar);
        Messenger<float>.AddListener(GameEvent.HANDLE_DAMAGE_ENVIRONMENT, OnHandleDamageEnvironement);
        Messenger<float>.AddListener(GameEvent.HANDLE_HEALING, OnHandleHealing);
        gameControl = GameControl.GetInstance();

        string filePath = "File/Level" + gameControl.currentLevel + "Features";
        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "healtBar":
                    startHealtBar = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("healtBar: " + startHealtBar);
                    break;
                case "startApples":
                    startApples = int.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("startApples: " + startApples);
                    break;
                case "startChickens":
                    startChickens = int.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("startChickens: " + startChickens);
                    break;
                case "powerUp":
                    powerUp = int.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("powerUp: " + powerUp);
                    break;
                case "powerUpCountDown":
                    powerUpCountDown = int.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("powerUpCountDown: " + powerUpCountDown);
                    break;
                default:
                    break;
            }
            
            }
        currentApples = startApples;
        currentChickens = startChickens;
        currentHealtBar = startHealtBar;
    }
    private void OnDestroy()
    {
        Messenger<Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
        Messenger<int>.RemoveListener(GameEvent.HANDLE_COLLECT_APPLE, OnHandleCollectApple);
        Messenger<int>.RemoveListener(GameEvent.HANDLE_COLLECT_CHICKEN, OnHandleCollectChicken);
        Messenger<float>.RemoveListener(GameEvent.HANDLE_HEALT_BAR, OnHandleHealtBar);
        Messenger<float>.RemoveListener(GameEvent.HANDLE_DAMAGE_ENVIRONMENT, OnHandleDamageEnvironement);
        Messenger<float>.RemoveListener(GameEvent.HANDLE_HEALING, OnHandleHealing);
        StopAllCoroutines();
    }
    public void OnHandleCollectApple(int count)
    {
        currentApples += count++;
        Debug.Log("Aggiorna Apple");
        Debug.Log(count);
        Debug.Log(currentApples);
        int amount = currentApples;
        if (amount >= powerUp)
        {
            Messenger.Broadcast(GameEvent.HANDLE_POWERUP_ON);
            StartCoroutine(powerUpHandle());
            currentApples = 0;
        }
    }
    IEnumerator powerUpHandle()
    {
        Debug.Log("PowerUp on");
        countDown = powerUpCountDown;
        while (countDown >= 0)
        {
            yield return new WaitForSeconds(1);
            countDown -= 1;
        }
        Messenger.Broadcast(GameEvent.HANDLE_POWERUP_OFF);
        Debug.Log("PowerUp off");
    }
    
    public void OnHandleCollectChicken(int count)
    {
        int amount =  count++;
        currentChickens += amount;
        Debug.Log("Aggiorna Chickens");
    }
    public void OnHandleHealtBar(float damageMult)
    {
        float amount = Time.deltaTime * damageMult;
        currentHealtBar -= amount;
        Debug.Log("Danno alla vita");
    }
    public void OnHandleDamageEnvironement(float damageMult)
    {
        float amount = Time.deltaTime * damageMult;
        currentHealtBar -=amount;
        Debug.Log("Danno alla vita traps");
    }
    public void OnHandleHealing(float amount)
    {
        if (Mathf.Approximately(currentHealtBar,startHealtBar))
        {
            return;   
        }else {
            currentHealtBar += amount;
            Debug.Log("Healing");
        }
    }
    public void OnHandleDamage(Damage damage)
    { 
        float amount = damage.Amount;
        currentHealtBar -=  amount;
    }
}
