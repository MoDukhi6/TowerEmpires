using UnityEngine;
using UnityEngine.UI;

public class SwitchImages : MonoBehaviour
{
    public GameObject firstImageObject;  // Reference to the first GameObject containing the image
    public GameObject secondImageObject; // Reference to the second GameObject containing the image
    LevelMusic lvlMusic;

    private void Start()
    {
        // Initially, show the first image and hide the second image
        firstImageObject.SetActive(true);
        secondImageObject.SetActive(false);

        // Get the LevelMusic component (adjust the type if needed)
        lvlMusic = GetComponent<LevelMusic>();

        // Ensure LevelMusic component is not null
        if (lvlMusic == null)
        {
            //Debug.LogError("LevelMusic component not found on the object.");
        }
    }

    public void ToggleImages()
    {
        // Toggle the active state of the image game objects
        firstImageObject.SetActive(!firstImageObject.activeSelf);
        secondImageObject.SetActive(!secondImageObject.activeSelf);

        // Control audio based on the active state of secondImageObject
        if (secondImageObject.activeSelf)
        {
            // Turn off or set volume to zero for all audio sources
            AudioListener.volume = 0f;
        }
        else
        {
            // Restore audio volume (you might want to adjust the volume level here)
            AudioListener.volume = 1f;
        }

        // Alternatively, if you have a specific LevelMusic component
        // and you want to control audio through it, you can use:
        // if (lvlMusic != null)
        // {
        //     lvlMusic.ToggleAudio(!secondImageObject.activeSelf);
        // }
    }
}
