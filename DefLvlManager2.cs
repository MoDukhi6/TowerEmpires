using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefLvlManager2 : MonoBehaviour
{
    public static DefLvlManager2 main;

    public Transform DefStart;
    public Transform[] path;

    private void Awake()
    {
        main = this;
    }
}
