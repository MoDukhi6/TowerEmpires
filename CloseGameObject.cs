using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseGameObject : MonoBehaviour
{
    public GameObject imageObject; // Reference to the GameObject containing the image
    private Image imageComponent;

    //public GameObject openImage1;
    //public GameObject closeImage1;

    private void Start()
    {
        imageComponent = imageObject.GetComponent<Image>();
        imageObject.SetActive(true); // Initially, show the image
    }

    public void ToggleImage()
    {
        imageObject.SetActive(!imageObject.activeSelf);
        imageObject.SetActive(false);
    }
}

