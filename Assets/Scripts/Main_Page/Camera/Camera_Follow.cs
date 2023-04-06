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
   
    public enum FollowType
    {
        GotoObject,
        FollowPlayer,
        LateFollow,
    }

    [SerializeField]
    private FollowType followType;
    void Start()
    {
        followPlayer = true;
        offSet = transform.position - target.position;
    }

    public void SetEnumValue(FollowType value)
    {
        followType = value;
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
            case FollowType.FollowPlayer:
                //DataBase_Manager.instance.pm.canMove = true;
                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothSpeed * Time.deltaTime);
                break;
            case FollowType.GotoObject:
                //DataBase_Manager.instance.pm.canMove = false;
                transform.position = Vector3.Lerp(transform.position, Goto, smoothSpeed * Time.deltaTime);
                StartCoroutine("BackType");
                break;
            case FollowType.LateFollow:
                //DataBase_Manager.instance.pm.canMove = false;
                transform.position = transform.position;
                StartCoroutine("BackType");
                break;
        }
    }
    IEnumerator BackType()
    {
        yield return new WaitForSeconds(3f);
        followType = FollowType.FollowPlayer;
    }
}
