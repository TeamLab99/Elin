using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    public GameObject crankMsg;
    SpriteRenderer spr;
    public GameObject crankWall;
    public Sprite crankUp;
    public Sprite crankDown;
    bool isCrankUp;
    bool isCrankCollide;
    bool isPullCrank;
    private void Awake()
    {
        spr=GetComponent<SpriteRenderer>();
        isCrankUp = true;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            if(isCrankCollide)
                isPullCrank = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCrankCollide = true;
            crankMsg.SetActive(true);
            if (isCrankUp&&isPullCrank)
            {
                crankWall.SetActive(false);
                spr.sprite = crankDown;
                isCrankUp = false;
            }
            else if(!isCrankUp&&isPullCrank)
            {
                crankWall.SetActive(true);
                spr.sprite = crankUp;
                isCrankUp = true;
            }
        }   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCrankCollide = false;
            crankMsg.SetActive(false);
            isPullCrank = false;
        }
    }
}
