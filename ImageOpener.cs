using UnityEngine;
using UnityEngine.UI;

public class ImageOpener : MonoBehaviour
{
    //public Image imageToOpen;
    //public Sprite imageSprite;
    public Button button;
    public Image image;
    public Sprite newSprite;
    void Start()
    {
        // Ensure the button and image are assigned in the Inspector.
        if (button != null && image != null)
        {
            // Add a click event listener to the button.
            button.onClick.AddListener(ChangeImageSprite);
        }
    }

    void ChangeImageSprite()
    {
        // Change the sprite of the UI Image to the new sprite.
        if (image != null)
        {
            image.sprite = newSprite;
        }
    }
    
    /*
    public void OpenImage()
    {
        // Set the sprite of the Image component to the desired image
        imageToOpen.sprite = imageSprite;
    }
    */
}
