using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    
    private Text abilityText;
    [SerializeField] string[] abilityTypeTexts;
    [SerializeField] Image elementImage;
    [SerializeField] Sprite[] elementTypeImages;
    [SerializeField] GameObject loadingObject;

    //[SerializeField] Transform playerPos;
    //private RectTransform rectTransform;
    //private Vector3 characterScreenPosition;
    //private Vector3 upVec = new Vector3(0, 2f, 0);

    private void Awake()
    {
        abilityText = GetComponentInChildren<Text>();
      
        //rectTransform = GetComponent<RectTransform>();
    }

    private void Start() // 초기화
    {
        ChangeAbilityType(0); 
        ChangeElementType(ESorting.None); 
    }

    public void ChangeElementType(ESorting _estort)
    {
        elementImage.sprite = elementTypeImages[(int)(_estort)];
    }

    public void ChangeAbilityType(int _idx)
    {
        abilityText.text = abilityTypeTexts[_idx];
    }

    public void SetLoadingEffect(bool active)
    {
        loadingObject.SetActive(active);
    }

    /*void UpdatePosition()
    {
        characterScreenPosition = Camera.main.WorldToScreenPoint(playerPos.position + upVec);
        rectTransform.position = characterScreenPosition;
    }*/
}
