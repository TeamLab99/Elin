using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//다양한 씬을 관리하기 위한 매니저인데 SceneManger라는 이름은 이미 존재하기에 SceneManagerEx라는 이름이다.
//현재 내가 사용할 일은 딱히 없다.
public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(EScene type)
    {
        //씬을 로드할 때 이미 생성된 데이터들을 싹 날리고 로드한다.
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    //현재 씬의 이름을 알 수 있는 함수
    string GetSceneName(EScene type)
    {
        string name = System.Enum.GetName(typeof(EScene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
