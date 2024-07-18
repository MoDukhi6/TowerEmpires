using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cashfopanel : MonoBehaviour
{
    [SerializeField] private Text takeTheCashTxt;
    [SerializeField] private Text toShow;

    void Start()
    {
        // Assign the text content, not the reference
        toShow.text = takeTheCashTxt.text;
    }
}
