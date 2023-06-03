using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Sprite[] abilityImages;
    private Image image;
    private RectTransform rectTransform;
    private Vector3 characterScreenPosition;
    private Vector3 upVec = new Vector3(0, 2f, 0);


    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
        characterScreenPosition = Camera.main.WorldToScreenPoint(playerPos.position + upVec);
        rectTransform.position = characterScreenPosition;
    }

    public void ShowAbilityUI(int _idx)
    {
        image.sprite = abilityImages[_idx];
    }
}
