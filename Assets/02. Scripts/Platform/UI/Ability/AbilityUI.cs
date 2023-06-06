using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    
    private Text text;
    [SerializeField] Image elementImage;
    [SerializeField] Sprite[] elementTypeImages;

    //[SerializeField] Transform playerPos;
    //private RectTransform rectTransform;
    //private Vector3 characterScreenPosition;
    //private Vector3 upVec = new Vector3(0, 2f, 0);

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        //rectTransform = GetComponent<RectTransform>();
    }


    public void ChangeElementType(ESorting _estort)
    {
        elementImage.sprite = elementTypeImages[(int)(_estort)];
    }

    /*void UpdatePosition()
    {
        characterScreenPosition = Camera.main.WorldToScreenPoint(playerPos.position + upVec);
        rectTransform.position = characterScreenPosition;
    }*/
}
