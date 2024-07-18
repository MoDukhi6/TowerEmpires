using UnityEngine;
using UnityEngine.UI;

public class Closeself : MonoBehaviour
{
    public GameObject openImageObject;  // Reference to the GameObject containing the image to open
    public GameObject closeImageObject; // Reference to the GameObject containing the image to close

    public void SwitchImages()
    {
        openImageObject.SetActive(true);
        closeImageObject.SetActive(false);
    }
}