using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene() 
    {
        SceneManager.LoadScene("MainGame");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public void LoadStartScene() 
    {
        SceneManager.LoadScene("StartScene");
    }

   public void ExitGame()
    {
        Application.Quit();
    }
}
