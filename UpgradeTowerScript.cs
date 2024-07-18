using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTowerScript : MonoBehaviour
{
    [SerializeField] CurencyScript currencyScript;

    public int upgradeCost;
    private int currencyInInt;
    private bool canUpgrade = false;

    public GameObject toOpen; // Reference to the GameObject containing the image
    private Image imageComponent;
    public GameObject toCloseTower; // Reference to the GameObject containing the image

    public GameObject toClosePanel; // Reference to the GameObject containing the image


    // Start is called before the first frame update
    void Start()
    {



        imageComponent = toOpen.GetComponent<Image>();
        toOpen.SetActive(false); // Initially, hide the image
        imageComponent = toCloseTower.GetComponent<Image>();
        toCloseTower.SetActive(true); // Initially, show the image
        imageComponent = toClosePanel.GetComponent<Image>();
        toClosePanel.SetActive(true); // Initially, show the image

        int currfromScript = currencyScript.GetCurrency();
        currencyInInt = int.Parse(currfromScript.ToString());
    }

    // Update is called once per frame
    public void Upgrade()
    {
        int currfromScript = currencyScript.GetCurrency();

        if (currfromScript >= upgradeCost)
        {
            canUpgrade = true;

            toOpen.SetActive(!toOpen.activeSelf);
            toCloseTower.SetActive(!toCloseTower.activeSelf);
            toCloseTower.SetActive(false);
            toClosePanel.SetActive(!toClosePanel.activeSelf);
            toClosePanel.SetActive(false);

            currencyScript.ChangeCurrency(currfromScript - upgradeCost);
        }
        else
        {
            Debug.Log("You don't have enough coins!");
        }
        canUpgrade = false;
    }
}
