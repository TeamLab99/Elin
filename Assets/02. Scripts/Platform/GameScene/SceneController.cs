using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public PlayerController playerController;
    

    public virtual void Awake() // Start로 수정하기
    {
        if(playerController==null)
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void Init() // Init을 override하는 걸로 수정하기 .. 등
    {

    }

    
}
