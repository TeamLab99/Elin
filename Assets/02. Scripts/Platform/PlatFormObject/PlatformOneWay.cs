using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOneWay : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentOneWayPlatform != null)
                StartCoroutine("Disable");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DownPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DownPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    IEnumerator Disable()
    {
        CompositeCollider2D platform = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platform,true);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreCollision(playerCollider, platform,false);
    }
}