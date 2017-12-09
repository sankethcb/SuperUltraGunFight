using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

#region variables
    public static GameManager instance = null;
    private string[] levels = { };
    private int currentLevel;
#endregion

    void Awake()
    {
        currentLevel = 0;
        if (instance == null)
        {
            instance = this;
           
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }



        DontDestroyOnLoad(gameObject);

    }

    public void LoadNextScene()
    {

    }

}
