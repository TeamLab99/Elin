using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱글톤 방식으로 구현하기 위해 모든 매니저들을 가지고 있는 스크립트이다.
public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }
    
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    

    void Start()
    {
        Init();
    }

    // 키보드 및 마우스 입력은 계속 검사를 해야하므로
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        //@Managers라는 오브젝트가 없다면 자동으로 생성해준다.
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            //DontDestroyOnLoad로 생성해줌
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            //Init을 호출해야하는 스크립트들의 Init 호출
            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }   
    }

    public static void Clear()
    {
        //클리어를 해줘야하는 스크립트들을 일괄적으로 클리어 해줌
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
        
    }
}
