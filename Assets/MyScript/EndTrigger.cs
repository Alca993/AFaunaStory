using System.Collections;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private GameControl gameManager;
    private static EndTrigger _instance;
    protected bool hasPlayerWon=false;

    public static EndTrigger Instance()
    {

        if (_instance == null)
            _instance = FindObjectOfType<EndTrigger>();
        return _instance;
    }
    public bool HasPlayerWon
    {
        get
        {
            return hasPlayerWon;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasPlayerWon = true;
        }
    }
}
