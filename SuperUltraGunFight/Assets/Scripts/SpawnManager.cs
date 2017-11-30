using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int playerNum;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    // Use this for initialization
    void Start ()
    {
        playerNum = GameObject.Find("PlayerNumberManager").GetComponent<PlayerNumberManager>().playerNum;

        //Spanw the players based on the player number
        if (playerNum == 2)
        {
            Instantiate(player1, new Vector3(-4.0f, 1.0f, 0.0f), Quaternion.identity);
            Instantiate(player2, new Vector3(4.0f, 1.0f, 0.0f), Quaternion.identity);
        }
        else if (playerNum == 3)
        {
            Instantiate(player1, new Vector3(-4.0f, 1.0f, 0.0f), Quaternion.identity);
            Instantiate(player2, new Vector3(4.0f, 1.0f, 0.0f), Quaternion.identity);
            Instantiate(player3, new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
    }
}
