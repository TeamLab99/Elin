using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("DisappearApple", 1f);
            PlatformEventManager.instance.FallEvent();
        }
    }

   private void DisappearApple()
    {
        gameObject.SetActive(false);
    }
}
