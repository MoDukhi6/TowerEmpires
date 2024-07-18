using UnityEngine;
using UnityEngine.UI;

public class MakeItClosed : MonoBehaviour
{
    public GameObject imageObject; // Reference to the GameObject containing the image
    private Image imageComponent;

    private void Start()
    {
        imageComponent = imageObject.GetComponent<Image>();
        imageObject.SetActive(false); // Initially, hide the image
    }
}