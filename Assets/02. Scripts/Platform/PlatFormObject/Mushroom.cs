using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("AddForce");
            if(playerController == null)
                playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.JumpMushroom();
        }
    }

}
