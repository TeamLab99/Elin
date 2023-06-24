using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator anim;
    PlayerMove playerMove;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("AddForce");
            if(playerMove ==null)
                playerMove = collision.gameObject.GetComponent<PlayerMove>();
            playerMove.JumpMushroom();
        }
    }

}
