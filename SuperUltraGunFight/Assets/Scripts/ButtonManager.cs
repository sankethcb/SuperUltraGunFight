using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    GameObject gameManager;
    public void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameManager.GetComponent<GameManager>().LoadNextScene();
        }
    }

    public void ChangeScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
