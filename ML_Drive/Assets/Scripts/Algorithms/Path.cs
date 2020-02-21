using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public static Path instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject[] mNodes;
    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject[] getPath()
    {
        return mNodes;
    }
    
    //Update is called every frame.
    void Update()
    {

    }
}
