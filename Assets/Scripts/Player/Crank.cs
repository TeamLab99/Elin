using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    SpriteRenderer spr;
    public GameObject crankWall;
    public Sprite crankUp;
    public Sprite crankDown;
    bool isCrankUp;
    private void Awake()
    {
        spr=GetComponent<SpriteRenderer>();
        isCrankUp = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isCrankUp)
            {
                crankWall.SetActive(false);
                spr.sprite = crankDown;
                isCrankUp = false;
            }
            else
            {
                crankWall.SetActive(true);
                spr.sprite = crankUp;
                isCrankUp = true;
            }
        }   
    }
}
