using UnityEngine;
using UnityEngine.UI;
using System;

public class SetTowerX : MonoBehaviour
{
    public GameObject imageObject; // Reference to the GameObject containing the image
    private Image imageComponent;
    //private int Currency = PriceTower.Price;
    

    [SerializeField] Text currencyInText;
    private int resultIntCurrency;
    [SerializeField]
    private Text TPriceInText;
    public int TPriceInInt;
    public static int newcur;
    [SerializeField]
    private int Currency;

    //private Tower selectedTower; // The current selected tower.

    /// <summary>
    public GameObject openImage1;  // Reference to the GameObject containing the image to open
    public GameObject closeImage1; // Reference to the GameObject containing the image to close
    public GameObject openImage2;
    public GameObject closeImage2;
    /// </summary>
    
    
    private void Start()
    {
        string ftextString = currencyInText.text;
        resultIntCurrency = int.Parse(ftextString);
        string stextString = TPriceInText.text;
        TPriceInInt = int.Parse(stextString);

        imageComponent = imageObject.GetComponent<Image>();
        imageObject.SetActive(false); // Initially, hide the image
        //Debug.Log("00resultIntCurrency is: " + resultIntCurrency);
        Currency = 270; // Initial currency value
        resultIntCurrency = Currency;
        //Debug.Log("01resultIntCurrency is: " + resultIntCurrency);
        currencyInText.text = Currency.ToString();
        //Debug.Log("resultIntCurrency is: " + resultIntCurrency + " Currency is: " + Currency + " currencyInText.text is: " + currencyInText.text);
    }

    public void ToggleImage()
    {
        if (Currency >= TPriceInInt)
        {
            //Debug.Log("Currency " + Currency + " TowerPrice: " + TPriceInInt);
            imageObject.SetActive(!imageObject.activeSelf);//imageComponent.enabled = !imageComponent.enabled;
            openImage1.SetActive(true);
            closeImage1.SetActive(false);
            openImage2.SetActive(true);
            closeImage2.SetActive(false);

            //Debug.Log("resultIntCurrency is: " + resultIntCurrency);

            if (Currency - TPriceInInt < 0)
            {
                Currency = 0;
                UpdateCurrencyText();
            }
            else
            {
                Currency = Currency - TPriceInInt;
                UpdateCurrencyText();
            }
        }
        else
        {
            Debug.Log("You don't have enougth coins!");
        }
    }

    public void IncreaseCurrency(int amount)
    {
        Currency += amount;
        UpdateCurrencyText();
    }

    public void IncreaseCurrencyForDeadEnemy(int killCoins)
    {
        Debug.Log("Entered IncreaseCurrency");
        Currency += killCoins;
        Debug.Log("Added Currency");
        UpdateCurrencyText();
        Debug.Log("Updated CurrencyTxt");
    }

    private void UpdateCurrencyText()
    {
        //Debug.Log("Updating currency text...");

        if (currencyInText != null)
        {
            currencyInText.text = Currency.ToString();
            //Debug.Log("__Currency: " + Currency);
            //Debug.Log("__currencyInText: " + currencyInText.text);
        }
        else
        {
            Debug.Log("currencyInText is null.");
        }
    }

}