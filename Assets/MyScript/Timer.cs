using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject timeDisplay1;
    public bool isTakingTime = false;
    public int theSeconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTakingTime == false)
        {
            StartCoroutine(AddSecond());
        }
    }
    IEnumerator AddSecond()
    {
        isTakingTime = true;
        theSeconds += 1;
        timeDisplay1.GetComponent<Text>().text = "" + theSeconds;
        yield return new WaitForSeconds(1);
        isTakingTime = false;
    }
}
