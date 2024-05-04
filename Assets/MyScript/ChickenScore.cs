using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChickenScore : MonoBehaviour
{
    public GameObject chickenCounterBox;
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
        chickenCounterBox.GetComponent<Text>().text = "" + intScore;
    }
}
