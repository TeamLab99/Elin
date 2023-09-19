using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    static PlatformManager platformManager;
    public static PlatformManager Instance { get { return platformManager; } }

    private void Awake()
    {
        if (platformManager == null)
        {
            GameObject go = GameObject.Find("PlatformManagers");
            if (go == null)
            {
                go = new GameObject { name = "PlatformManagers" };
                go.AddComponent<PlatformManager>();
            }
            platformManager = go.GetComponent<PlatformManager>();
            DontDestroyOnLoad(go);
        }
    }

    public void Clear()
    {
        PlayerPoolManager.instance.Clear();
        PlayerRespawnManager.instance.Clear();
    }

    public void Init()
    {
        
    }
}
