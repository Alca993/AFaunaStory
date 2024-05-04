using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class UpgradeSelector : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    public UnityEngine.UI.Button[] levelButtons;
    private string[] unlockedUpgrades;
    private int[] prices;
    private Utility.Utility util;
    private int upgradeToUnlock = -1;
    private int chickenCurrency;

    public GameObject confirmationPanel;
    public Text currentCurrency;
    private int upgradeSelected;
    private int indexRequestedUpgrade = 0;
    private bool confirmPurchase = false;
    private bool buyUpgrade = false;


    private void Start()
    {
        unlockedUpgrades = new string[levelButtons.Length];
        prices = new int[levelButtons.Length];
        util = Utility.Utility.Instance();

        chickenCurrency = PlayerPrefs.GetInt("ChickenCurrency", 0); //testing

        currentCurrency.text = "" + chickenCurrency;
        upgradeSelected = PlayerPrefs.GetInt("upgradeSelected", 1);
        
        for(int i = 0; i < levelButtons.Length; i++)
        {
            string filePath = "File/characterAttack" + (i + 1) + "Features";
            

            Image rangeImage = levelButtons[i].transform.Find("RangePanel").Find("RangeFront").GetComponent<Image>();
            Image rateImage = levelButtons[i].transform.Find("AttackRatePanel").Find("AttackFront").GetComponent<Image>();
            
            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split(NEW_LINE);

            for (int j = 0; j < lines.Length; j++)
            {
                string line = lines[j];
                string[] token = line.Split(EQUALS);

                switch (token[0])
                {
                    case "attackRange":
                        float attackRange = float.Parse(token[1], CultureInfo.InvariantCulture);
                        rangeImage.fillAmount = attackRange / util.Max_Range;
                        Debug.Log("attackRange: " + attackRange);
                        break;
                    case "attackRate":
                        float attackRate = float.Parse(token[1], CultureInfo.InvariantCulture);
                        rateImage.fillAmount = attackRate / util.Max_Rate;
                        Debug.Log("attackRate: " + attackRate);
                        break;
                    case "price":
                        int price = int.Parse(token[1], CultureInfo.InvariantCulture);
                        prices[i] = price;
                        Debug.Log("price: " + price);
                        break;
                    default:
                        break;
                }

            }

            if ((i+1)== upgradeSelected)
            {
                unlockedUpgrades[i] = "true";
                levelButtons[i].transform.Find("SelectPanel").gameObject.SetActive(true);
                levelButtons[i].transform.Find("LockPanel").gameObject.SetActive(false);
            }
            else
            {
                string isAvailable = PlayerPrefs.GetString("isUpgrade" + (i + 1) + "Unlocked", "false");

                if (i == 0)
                    isAvailable = "true";
                if (isAvailable == "true")
                {
                    unlockedUpgrades[i] = "true";
                    levelButtons[i].transform.Find("LockPanel").gameObject.SetActive(false);

                }
                else
                {
                    unlockedUpgrades[i] = "false";
                    GameObject pricePanel = levelButtons[i].transform.Find("PricePanel").gameObject;
                    pricePanel.transform.Find("Value").GetComponent<Text>().text = "" + prices[i];
                    pricePanel.SetActive(true);
                    if (prices[i] > chickenCurrency)
                        levelButtons[i].interactable = false;
                }
            }
        }
    }
  private void Update()
    {
        if (confirmPurchase)
        {
            confirmationPanel.SetActive(true);
            Debug.Log("Panel Conferma attivo");
            if (buyUpgrade)
            {
                Debug.Log("Buy true");
                Buy();
                confirmPurchase = false;
                buyUpgrade = false;
            }
        }
        else
        {
            confirmationPanel.SetActive(false);
        }
        if (indexRequestedUpgrade <= 0) return;


        
        if (unlockedUpgrades[indexRequestedUpgrade - 1] == "true")
            SelectUpgrade(indexRequestedUpgrade - 1);
        else
        {
            
            UnlockUpgrade(indexRequestedUpgrade - 1);
        }

        indexRequestedUpgrade = 0;

    }

    public void Select(int i)
    {

        indexRequestedUpgrade = i;

    }
    private void SelectUpgrade(int index)
    {
        levelButtons[upgradeSelected - 1].transform.Find("SelectPanel").gameObject.SetActive(false);
        levelButtons[index].transform.Find("SelectPanel").gameObject.SetActive(true);

        upgradeSelected = index + 1;
        PlayerPrefs.SetInt("upgradeSelected", upgradeSelected);
    }
    private void UnlockUpgrade(int index)
    {
        Image img = levelButtons[index].transform.Find("ImagePanel").gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        confirmationPanel.transform.Find("Upgrade").gameObject.GetComponent<Image>().sprite = img.sprite;

        upgradeToUnlock = index;
        confirmPurchase = true;
    }
    private void Buy()
    {
        if (chickenCurrency >= prices[upgradeToUnlock])
        {
            chickenCurrency -= prices[upgradeToUnlock];
            PlayerPrefs.SetInt("ChickenCurrency", chickenCurrency);

            PlayerPrefs.SetString("isWeapon" + (upgradeToUnlock + 1) + "Unlocked", "true");
            unlockedUpgrades[upgradeToUnlock] = "true";


            levelButtons[upgradeToUnlock].transform.Find("LockPanel").gameObject.SetActive(false);
            levelButtons[upgradeToUnlock].transform.Find("PricePanel").gameObject.SetActive(false);

            currentCurrency.text = "" + chickenCurrency;
            confirmPurchase = false;
        }
        else
        {
            confirmationPanel.transform.Find("Log").gameObject.SetActive(true);
        }
    }
    public void Confirm()
    {
        buyUpgrade = true;

    }

    public void Cancel()
    {

        buyUpgrade = false;
        confirmPurchase = false;
    }

    public void BackButton()
    {

        Debug.Log("Back pressed");
        GameControl.UnloadUpgrades();
    }
}
