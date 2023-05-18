using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public void LoadSceneSingle(int _Idx)
    {
        SceneManager.LoadScene(_Idx);
    }

    public void LoadSceneAddtive(int _idx)
    {
        SceneManager.LoadScene(_idx, LoadSceneMode.Additive);
    }
}