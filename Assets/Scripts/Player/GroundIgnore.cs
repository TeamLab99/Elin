using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIgnore : MonoBehaviour
{
    public Collider2D platfomCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>() ,platfomCollider , true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platfomCollider, false);
        }
    }
}
