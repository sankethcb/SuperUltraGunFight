using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

#region variables
    public static GameManager instance = null;
    private string[] levels = { "Scenes/Main_Scene", "Scenes/Levels/Level1", "Scenes/Levels/Level2" , "Scenes/Levels/Level3" , "Scenes/Levels/Level4", "Scenes/Levels/Level5" };
   // private string[] levels = { "Scenes/Main_Scene" };
    private int currentLevel;
    bool gameplay = false;
    public GameObject scoreMangaer;

    #endregion

    private void Awake()
    {
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

    void OnLevelWasLoaded(int level)
    {
        if (gameplay)
        {
            scoreMangaer = GameObject.Find("ScoreManager");
        }
       
       

    }

    public void Update()
    {
        if (gameplay)
        {
            if (scoreMangaer.GetComponent<ScoreManager>().player1ScoreVal <= 0 || scoreMangaer.GetComponent<ScoreManager>().player2ScoreVal <= 0)
            {
                Debug.Log(scoreMangaer.GetComponent<ScoreManager>().player1ScoreVal);
                LoadNextScene();
            }
        }
    }

    public void LoadNextScene()
    {
        
        int newLevel;
        do
        {
            newLevel = Random.Range(1, 6);
        } while (newLevel == currentLevel);
        currentLevel = newLevel;
        gameplay = true;
        SceneManager.LoadScene(levels[newLevel], LoadSceneMode.Single);
    }

}
