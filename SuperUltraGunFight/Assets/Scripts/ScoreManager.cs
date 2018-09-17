using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Canvas myCanvas;

    Text player1Score;
    Text player2Score;
    Text player3Score;
    Text player4Score;
    //Text player3Score;

    Transform player1Wins;
    Transform player2Wins;
    Transform player3Wins;
    Transform BackToSelectionButton;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    //public GameObject player3;

    public int player1ScoreVal = 10;
    public int player2ScoreVal = 10;
    public int player3ScoreVal = 10;
    public int player4ScoreVal = 10;
    //int player3ScoreVal;

    // Use this for initialization
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player3 = GameObject.FindGameObjectWithTag("Player3");
        player4 = GameObject.FindGameObjectWithTag("Player4");

        if(player3 == null)
        {
            player3ScoreVal = 0;
        }
        if(player4 == null)
        {
            player4ScoreVal = 0;
        }

        //player3 = GameObject.FindGameObjectWithTag("Player3");

        player1Score = myCanvas.transform.Find("Player1 Score").GetComponent<Text>();
        player2Score = myCanvas.transform.Find("Player2 Score").GetComponent<Text>();
        player3Score = myCanvas.transform.Find("Player3 Score").GetComponent<Text>();
        player4Score = myCanvas.transform.Find("Player4 Score").GetComponent<Text>();
        //player3Score = myCanvas.transform.Find("Player3 Score").GetComponent<Text>();

        player1Wins = myCanvas.transform.Find("Player1 Wins");
        player2Wins = myCanvas.transform.Find("Player2 Wins");
        //player3Wins = myCanvas.transform.Find("Player3 Wins");
        BackToSelectionButton = myCanvas.transform.Find("BackToSelectionButton");
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null) //If the player is not destroyed, get the score and print to the screen
        {
            player1ScoreVal = player1.GetComponent<PlayerScore>().score;
            player1Score.text = player1ScoreVal.ToString();
        }
        else //If the player is destroyed, then the score is 0
        {
            player1ScoreVal = 0;
            player1Score.text = "0";
        }
        if (player2 != null)
        {
            player2ScoreVal = player2.GetComponent<PlayerScore>().score;
            player2Score.text = player2ScoreVal.ToString();
        }
        else
        {
            player2ScoreVal = 0;
            player2Score.text = "0";
        }
        if (player3 != null) //If the player is not destroyed, get the score and print to the screen
        {
            
            player3ScoreVal = player3.GetComponent<PlayerScore>().score;
            player3Score.text = player3ScoreVal.ToString();
        }
        else //If the player is destroyed, then the score is 0
        {
            player3ScoreVal = 0;
            player3Score.text = "0";
        }
        if (player4 != null)
        {
            player4ScoreVal = player4.GetComponent<PlayerScore>().score;
            player4Score.text = player4ScoreVal.ToString();
        }
        else
        {
            player4ScoreVal = 0;
            player4Score.text = "0";
        }
        //if (player3 != null)
        //{
        //    player3ScoreVal = player3.GetComponent<PlayerScore>().score;
        //    player3Score.text = player3ScoreVal.ToString();
        //}
        //else
        //{
        //    player3ScoreVal = 0;
        //    player3Score.text = "0";
        //}

        ////If 2 out of 3 players have 0 score, the other one wins
        //if (player1ScoreVal == 0 && player2ScoreVal == 0)
        //{
        //    player3Wins.gameObject.SetActive(true);
        //    BackToSelectionButton.gameObject.SetActive(true);
        //}
        //else if (player1ScoreVal == 0 && player3ScoreVal == 0)
        //{
        //    player2Wins.gameObject.SetActive(true);
        //    BackToSelectionButton.gameObject.SetActive(true);
        //}
        //else if (player2ScoreVal == 0 && player3ScoreVal == 0)
        //{
        //    player1Wins.gameObject.SetActive(true);
        //    BackToSelectionButton.gameObject.SetActive(true);
        //}
    }
}
