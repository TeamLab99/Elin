using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamerEffectType
{
    FollowPlayer,
    LateFollowPlayer,
    GotoTarget,
    GameMode
}

public class CamerEffect : Singleton<CamerEffect>
{
    public Transform target;
    public Transform apple;
    public Transform player;
    public float lerpTime;
    public CamerEffectType camerEffectType;

    private Vector3 targetCamPos;
    private Vector3 offSet;

    private float currentTime = 0f;
    private bool onCamera = true;

    private void Awake()
    {
        offSet = target.position - transform.position;
        target = player;
    }

    void Update()
    {
        targetCamPos = target.position;
        targetCamPos.y += 5f;
        targetCamPos.z = -10f;
        switch (camerEffectType)
        {
            case CamerEffectType.FollowPlayer: // 디폴트
                currentTime += Time.deltaTime;
                if (currentTime >= lerpTime)
                    currentTime = lerpTime;
                transform.position = Vector3.Lerp(transform.position, targetCamPos, currentTime / lerpTime);
                break;
            case CamerEffectType.GotoTarget:
                currentTime += Time.deltaTime;
                if (currentTime >= lerpTime)
                    currentTime = lerpTime;
                transform.position = Vector3.Lerp(transform.position, targetCamPos, currentTime / lerpTime);
                if (onCamera)
                    Invoke("ChangeFollowCameraMode", 3f);
                onCamera = false;
                break;
            case CamerEffectType.LateFollowPlayer:
                transform.position = transform.position;
                if (onCamera)
                    Invoke("ChangeFollowCameraMode", 3f);
                onCamera = false;
                break;
            case CamerEffectType.GameMode: // 게임 들어갈 때 카메라 효과
                break;
        }
    }

    public void ChangeLateFollowCamerMode() // 나무에서 떨어질때
    {
        camerEffectType = CamerEffectType.LateFollowPlayer;
        PlatformEventManager.instance.ControlPlayerMove(false);
    }

    public void ChangeGotoCameraMode() // 나무 위의 사과를 비출 때
    {
        currentTime = 0f;
        target = apple;
        camerEffectType = CamerEffectType.GotoTarget;
        PlatformEventManager.instance.ControlPlayerMove(false);
    }

    public void ChangeFollowCameraMode()
    {
        currentTime = 0f;
        target = player;
        onCamera = true;
        camerEffectType = CamerEffectType.FollowPlayer;
        PlatformEventManager.instance.ControlPlayerMove(true);
    }
}
