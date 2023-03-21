using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Event : MonoBehaviour
{
    public Camera_Follow cameraFollow;
    public Vector3 DesVec;
    private bool isTry=true;
    public EventType eventType;
    public enum EventType{
        Goto,
        Follow,
        Late
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTry)
        {
            switch (eventType)
            {
                case EventType.Goto:
                    DataBase_Manager.instance.cm.InputVec(DesVec);
                    cameraFollow.SetEnumValue(Camera_Follow.FollowType.GotoObject);
                    break;
                case EventType.Follow:
                    cameraFollow.SetEnumValue(Camera_Follow.FollowType.FollowPlayer);
                    break;
                case EventType.Late:
                    cameraFollow.SetEnumValue(Camera_Follow.FollowType.LateFollow);
                    break;
            }
            isTry = false;
        }
    }
          
}