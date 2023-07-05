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

    public void LoadStatScene() // 게임씬
    {
        SceneManager.LoadScene("MainGame");
    }

    public void LoadGameScene() // 시작씬
    {
        SceneManager.LoadScene("StartScene");
    }
}
