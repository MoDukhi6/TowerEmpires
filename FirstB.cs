using UnityEngine;
using UnityEngine.UI;

public class FirstButtonScript : MonoBehaviour
{
    public Button secondButton; // Reference to the second button

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OpenSecondButton);
    }

    // Function to open the second button
    private void OpenSecondButton()
    {
        secondButton.gameObject.SetActive(true);
    }
}
