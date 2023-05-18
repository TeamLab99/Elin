using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    public GameObject[] destroyTile;
    public Camera_Event cm;
   
    //    private bool isObj; // 객체와 붙어있는가?
    //    public float objDist; // 객체와의 거리 
    //public LayerMask objLayer; // 객체 레이어
    //isObj = Physics2D.Raycast(checkPosition.position, Vector2.right* isRight, objDist, objLayer); // 바라보는 방향에 객체가 있는지 확인
    /*
     *  if (isObj)
        {
            switch (collision.gameObject.layer)
            {
                case 14: // NPC
                    
                    break;
                case 15: // 아이템 박스
                    break;
                case 31:
                    playerInteract.DestroyBlock();
                    break;
            }
        }
    if (scanObject != null && Input.GetKeyDown(KeyCode.Q))
        {
            NPC scanNPC = scanObject.GetComponent<NPC>();
            scanNPC.ShowQuestText();
        }  
        private GameObject scanObject;
    if (rayNpc.collider != null)
            scanObject = rayNpc.collider.gameObject;
        else
            scanObject = null;
      if (isRight == 1)
            frontDir = Vector2.right;
        else if (isRight == -1)
            frontDir = Vector2.left;
        RaycastHit2D rayNpc = Physics2D.Raycast(rb.position, frontDir, 1f, LayerMask.GetMask("NPC"));
     */
}
