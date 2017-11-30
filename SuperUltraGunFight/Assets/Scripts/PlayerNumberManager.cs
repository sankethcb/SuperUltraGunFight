using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberManager : MonoBehaviour
{
    public int playerNum;
    
    public void Awake()
    {
        //The object with this function will stay when the scene changes
        DontDestroyOnLoad(gameObject);
    }


	public void NumSelect(int num)
    {
        playerNum = num;
    }
}
