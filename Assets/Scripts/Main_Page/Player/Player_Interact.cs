using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    public GameObject[] destroyTile;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==15)
        {
            DataBase_Manager.instance.cm.IsFollow();
            for(int i=0; i < destroyTile.Length-1; i++)
            {
                Debug.Log("dd");
                destroyTile[i].SetActive(false);
                StartCoroutine("ReFollowPlayer");
            }
        }
    }

    IEnumerator ReFollowPlayer()
    {
        yield return new WaitForSeconds(1f);
        destroyTile[2].SetActive(false);
        yield return new WaitForSeconds(3f);
        DataBase_Manager.instance.cm.followPlayer = true;
    }
}
