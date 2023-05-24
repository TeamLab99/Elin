using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    SpriteRenderer spr;
    [SerializeField] Sprite image;
    [SerializeField] int boxID;
    bool isOpen = false;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        if (PlayerPrefs.HasKey("BoxState" + boxID))
        {
            if (PlayerPrefs.GetInt("BoxState" + boxID) == 1)
                OpenBox();
            else
                isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isOpen)
            {
                OpenBox();
                SaveBoxState(); // 상자의 상태를 저장합니다.
            }
        }
    }

    void OpenBox()
    {
        isOpen = true;
        spr.sprite = image;
    }

    void SaveBoxState()
    {
        PlayerPrefs.SetInt("BoxState" + boxID, isOpen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
