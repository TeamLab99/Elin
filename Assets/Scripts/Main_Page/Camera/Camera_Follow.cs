using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public bool followPlayer;
    private Vector3 offSet;
    private Vector3 Goto;
    public FollowType followType;
    public enum FollowType
    {
        GotoObject,
        FollowPlayer,
        LateFollow
    }
    void Start()
    {
        followPlayer = true;
        offSet = transform.position - target.position;
    }

    public void InputVec(Vector3 inputVec)
    {
        Goto = inputVec;
    }
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offSet;
        targetCamPos.z = -15f;
        Goto.z = -15f;
        switch (followType)
        {
            case FollowType.GotoObject:
                transform.position = Vector3.Lerp(transform.position, Goto, smoothSpeed * Time.deltaTime);
                StartCoroutine("BackType");
                break;
            case FollowType.FollowPlayer:
                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothSpeed * Time.deltaTime);
                break;
            case FollowType.LateFollow:
                transform.position = transform.position;
                break;
        }
    }
    IEnumerator BackType()
    {
        yield return new WaitForSeconds(3f);
        followType = FollowType.FollowPlayer;
    }
}
