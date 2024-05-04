using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleController : MonoBehaviour
{
    float rotSpeed = 180;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.World);
    }
}
