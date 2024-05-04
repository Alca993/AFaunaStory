using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject scoreBox;
    public static int currentScore;
    public int intScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        intScore = currentScore;
        scoreBox.GetComponent<Text>().text = "" + intScore;
    }
}
