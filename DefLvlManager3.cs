using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefLvlManager3 : MonoBehaviour
{
    public static DefLvlManager3 main;

    public Transform DefStart;
    public Transform[] path;

    private void Awake()
    {
        main = this;
    }
}
