using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Event : MonoBehaviour
{
    public Vector3 DesVec;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataBase_Manager.instance.cm.InputVec(DesVec);
            DataBase_Manager.instance.cm.followType = Camera_Follow.FollowType.GotoObject;
        }
    }
          
}
