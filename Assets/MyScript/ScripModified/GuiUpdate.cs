using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class GuiUpdate : MonoBehaviour
{
    private ObjectManager om;
    private GameControl gameControl;
    private PauseMenu pauseMenu;
    public Canvas canvas;
    [SerializeField] private Image healtBar;
    [SerializeField] private Image healtBarScore;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelWonUI;
    [SerializeField] Text apple;
    [SerializeField] Text chicken;
    [SerializeField] GameObject powerUpSignal;
    [SerializeField] Text timer;
    
    private float speed;
    private float startHB;
    private float scoreSpeed;
    private CharacterStatus charStatus;
   
    void Start()
    {
        om = ObjectManager.Instance();
        gameControl = GameControl.GetInstance();
        pauseMenu = PauseMenu.Instance();
        charStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStatus>();

        startHB = om.StartHealtBar;
        healtBar.fillAmount = om.CurrentHealtBar / startHB;
        chicken.text = om.CurrentChickens.ToString();
        apple.text = om.CurrentApples.ToString() + "/" + om.PowerUp.ToString();

        string filePath = "File/updateGuiFeatures";


        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split('=');

            switch (token[0])
            {
                case "speed":
                    speed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "scoreSpeed":
                    scoreSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        _GameState gameState = gameControl.GetGameState();
        float hb = om.CurrentHealtBar;

        float startHealtBar = healtBar.fillAmount;
        float endHealtBar = hb / startHB;

        healtBar.fillAmount = Mathf.Lerp(startHealtBar, endHealtBar, speed * Time.deltaTime);

        if (gameState == _GameState.Play)
        {
            if (pauseMenu.IsOn)
            {
                pauseMenu.Hide();
                FindObjectOfType<PlayerAttack>().enabled = true;
            }
        }else if (gameState == _GameState.Pause)
        {
            if (!pauseMenu.IsOn)
            {
                pauseMenu.Show();
                FindObjectOfType<PlayerAttack>().enabled = false;
            }
        }else if(gameState == _GameState.Over)
        {
            gameOverUI.SetActive(true);
        }else if(gameState == _GameState.Won)
        {
            StartCoroutine(LevelWon());
            float finalHeatlbar = om.CurrentHealtBar;
            Debug.Log("Healt bar" + finalHeatlbar);
        }
        chicken.text = om.CurrentChickens.ToString();
        apple.text = om.CurrentApples.ToString() + "/" + om.PowerUp.ToString();

        if (charStatus.IsPowerUp)
        {
            powerUpSignal.SetActive(true);
        }
        else
        {
            powerUpSignal.SetActive(false);
        }
    }
    private IEnumerator LevelWon()
    {
        levelWonUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);

        float firstTH = gameControl.FirstTh;
        float secondTH = gameControl.SecondTH;
        GameObject[] starBorders = GameObject.FindGameObjectsWithTag("score");

        float finalHealtBar = om.CurrentHealtBar;
        float startHealt = healtBarScore.fillAmount;
        float endHealtBar = finalHealtBar / startHB;

        healtBarScore.fillAmount = Mathf.Lerp(startHealt, endHealtBar, scoreSpeed * Time.fixedDeltaTime);
        
        while (healtBarScore.fillAmount >= endHealtBar)
        {
            if (healtBarScore.fillAmount < secondTH)
            {
                starBorders[2].transform.GetChild(0).gameObject.SetActive(false);
            }
            if (healtBarScore.fillAmount < firstTH)
            {
                starBorders[1].transform.GetChild(0).gameObject.SetActive(false);
            }

            startHealt = healtBarScore.fillAmount;


            healtBarScore.fillAmount = Mathf.Lerp(startHealt, endHealtBar, scoreSpeed * Time.fixedDeltaTime);
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        StopCoroutine(LevelWon());
    }
}
