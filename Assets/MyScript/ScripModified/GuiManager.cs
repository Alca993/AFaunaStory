using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private bool paused, over, won;
    private GameControl gameControl;
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameControl.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            OnPauseButton();
        }
    }
    void OnGUI()
    {
        paused = gameControl.IsGamePaused();
    }
    public void OnPauseButton()
    {
        _GameState gameState = gameControl.GetGameState();
        if (gameState == _GameState.Over) return;


        if (gameState == _GameState.Pause)
        {
            gameControl.ResumeGame();

        }
        else
        {
            gameControl.PauseGame();

        }
    }
}
