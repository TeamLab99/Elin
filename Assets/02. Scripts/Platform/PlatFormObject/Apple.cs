using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("DissappearApple", 1f);
        }
    }

    private void DissapearApple()
    {
        gameObject.SetActive(false);
    }
}
