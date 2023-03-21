using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    public GameObject[] destroyTile;
    public Camera_Event cm;
    public void DestroyBlock()
    {
        for (int i = 0; i < destroyTile.Length - 1; i++)
        {
            DataBase_Manager.instance.cm.SetEnumValue(Camera_Follow.FollowType.LateFollow);
            destroyTile[i].SetActive(false);
        }
    }
}
