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
            StartCoroutine("ReFollowPlayer");
        }
    }
    IEnumerator ReFollowPlayer()
    {
        yield return new WaitForSeconds(1f);
        destroyTile[2].SetActive(false);
        yield return new WaitForSeconds(3f);
        DataBase_Manager.instance.cm.SetEnumValue(Camera_Follow.FollowType.FollowPlayer);
    }
}
