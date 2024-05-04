using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputControllers;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Globalization;
using Character;

public enum _GameState { Play, Pause, Over, Won, Restart }
    public enum OperationRequested { Home, Continue, SelectLevel, Upgrade, Retry, Null }
    public class GameControl : MonoBehaviour
    {
        private const char NEW_LINE = '\n';
        private const char EQUALS = '=';
        protected static GameControl instance;
        public _GameState gameState = _GameState.Play;
        private ObjectManager om;
        private OperationRequested op = OperationRequested.Null;

        public int currentLevel;
        private float playerLife;
        private EndTrigger endLevel;
        private int levelReached;
        private float firstTH, secondTH;
        private int score = 0;
        public static GameControl GetInstance()
        {
            if (instance == null)
                instance = FindObjectOfType<GameControl>();
                return instance;
        }
        public float FirstTh
         {
            get
            {
                return firstTH;
            }
        }

        public float SecondTH
        {
            get
            {
                return secondTH;
            }
        }
        public int Score
        {
            get
            {
                return score;
            }
        }
        public bool IsGameOver()
        {
            return gameState == _GameState.Over ? true : false;
        }
        public bool IsGamePaused()
        {
            return gameState == _GameState.Pause ? true : false;
        }
        public _GameState GetGameState()
        {
            return gameState;
        }
        public void SetGameStateOver()
        {
            gameState = _GameState.Over;
            Time.timeScale = 0;
        }
        public bool HasPlayerWon()
        {
            return gameState == _GameState.Won ? true : false;
        }
        public void SetGameStateWon()
        {
            gameState = _GameState.Won;
            Time.timeScale = 0;
        }
          public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }

    public OperationRequested OpRequested
        {

            set
            {
                op = value;
            }


        }

        public static void LoadMainMenu()
        {
            Time.timeScale = 1;
            Load("MainMenuScene");
        }
        public static void Load(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        void Awake()
        {
            Time.timeScale = 1;
        }
    public void Start()
    {
        om = ObjectManager.Instance();
        endLevel = EndTrigger.Instance();
        levelReached = PlayerPrefs.GetInt("levelReached", 1);


        string filePath = "File/Level" + currentLevel + "Features";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "firstTH":
                    firstTH = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "secondTH":
                    secondTH = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }

        }
    }
    public void Update()
    {
        if (gameState == _GameState.Restart)
            Load(SceneManager.GetActiveScene().name);
            playerLife = om.CurrentHealtBar;
             if (playerLife<=0)
             {
                 EndGame();
             }else if(endLevel.HasPlayerWon)
             {
                 WonGame();
             }
        switch (op)
        {
            case OperationRequested.Home: LoadMainMenu(); break;
            case OperationRequested.Continue:
                Load("Level" + (currentLevel + 1)); break;
            case OperationRequested.SelectLevel:
                if (gameState == _GameState.Won)
                    StoreChickens();
                Load("LevelSelector"); break;
            case OperationRequested.Retry:
                if (gameState == _GameState.Won)
                    StoreChickens();
                Load(SceneManager.GetActiveScene().name);
                break;
            case OperationRequested.Upgrade: LoadUpgrades(); op = OperationRequested.Null; break;
            default: break;
        }
    }
    void WonGame()
        {

            SetGameStateWon();
            float finalHealtBar = om.CurrentHealtBar;
            float startHealtBar = om.StartHealtBar;
            float finalHealtBarPercentage = finalHealtBar / startHealtBar;

            if (finalHealtBarPercentage < firstTH)
            {
                score = 1;
            }
            else if (finalHealtBarPercentage >= firstTH && finalHealtBarPercentage < secondTH)
            {
                score = 2;   
            }
            else if (finalHealtBarPercentage>= secondTH)
            {
                score = 3;
            }

            int bestScore = PlayerPrefs.GetInt("starForLevel" + currentLevel, 0);
           
        if (score > bestScore)
                PlayerPrefs.SetInt("starForLevel" + currentLevel, score);

            if (currentLevel == levelReached)
            {
                levelReached += 1;
                PlayerPrefs.SetInt("levelReached", levelReached);
            }
           }
   
    private void StoreChickens()
    {
        PlayerPrefs.SetInt("ChickenCurrency", om.CurrentChickens);
    }
    
    public void PauseGame()
    {
        gameState = _GameState.Pause;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameState = _GameState.Play;
        Time.timeScale = 1;
        Debug.Log("GAME RESUMED");
    }

    public void EndGame()
    {

        SetGameStateOver();
        Debug.Log("Game Over");

    }

    public void SelectLevel()
    {

        op = OperationRequested.SelectLevel;
    }
    public void Retry()
    {

        op = OperationRequested.Retry;
    }

    public void Continue()
    {

        op = OperationRequested.Continue;
    }

    public static void LoadUpgrades()
    {
        SceneManager.LoadScene("UpgradeMenu",LoadSceneMode.Additive);
    }

    public static void UnloadUpgrades()
    {
        SceneManager.UnloadScene("UpgradeMenu");
    }
    public static void Quit()
    {
        Application.Quit();
    }
}
    
