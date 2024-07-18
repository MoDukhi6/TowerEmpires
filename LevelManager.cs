using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;
    public Transform startPoint2;
    public Transform[] path2;


    private void Awake()
    {
        main = this;
    }
}
