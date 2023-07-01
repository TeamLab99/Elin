using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] Image abilityImage;
    [SerializeField] Sprite seperateImage;
    [SerializeField] Sprite[] absorptionTypeImages;
    [SerializeField] Sprite[] projectileTypeImages;
    int currentState = 0;
    EProjectileType currenElement;
    //[SerializeField] Transform playerPos;
    //private RectTransform rectTransform;
    //private Vector3 characterScreenPosition;
    //private Vector3 upVec = new Vector3(0, 2f, 0);

    private void Start() // 초기화
    {
        ChangeAbilityType(0); 
        ChangeElementType(EProjectileType.None); 
    }

    public void ChangeElementType(EProjectileType _estort)
    {
        currenElement = _estort;
       switch (currentState)
        {
            case 1:
                abilityImage.sprite = absorptionTypeImages[(int)_estort];
                break;
            case 2:
                abilityImage.sprite = projectileTypeImages[(int)_estort];
                break;
            default:
                break;
        }
    }

    public void ChangeAbilityType(int _idx)
    {
        currentState = _idx;
        if (_idx == 0)
            abilityImage.sprite = seperateImage;
        else
            ChangeElementType(currenElement);
    }

    /*void UpdatePosition()
    {
        characterScreenPosition = Camera.main.WorldToScreenPoint(playerPos.position + upVec);
        rectTransform.position = characterScreenPosition;
    }*/
}
