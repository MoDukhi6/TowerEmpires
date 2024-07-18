using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStarsOpenLevel : MonoBehaviour
{
    public string levelName;
    private string oneStar = "1";
    private string twoStar = "2";
    private string threeStar = "3";
    // Start is called before the first frame update
    public void Set1Stars()
    {
        levelName = oneStar;
    }

    public void Set2Stars()
    {
        levelName = twoStar;
    }

    public void Set3Stars()
    {
        levelName = threeStar;
    }

}
