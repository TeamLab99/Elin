using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject textBox;
    public GameObject destroyGround;
    public Camera_Follow cameraFollow;
    bool isDown=false;
    private void Update()
    {
        if (Input.GetKey(KeyCode.X))
            isDown = true;
        else if (Input.GetKeyUp(KeyCode.X))
            isDown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            if (isDown)
            {
                Debug.Log("실행중");
                destroyGround.SetActive(false);
            }
            textBox.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textBox.SetActive(false);
        }
    }
}
