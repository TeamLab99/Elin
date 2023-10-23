using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 카드 가지고 있는 속성들의 정보를 담은 스크립트
/// </summary>
[System.Serializable]
public class MagicResources
{
    public int id;
    public string name;
    public GameObject skillEffect;
    public GameObject hitEffect;
    public Sprite cardImage;
}


// Items List를 인스펙터창에서 접근할 수 있는 파일로 만들었다.
// 이 리스트를 접근하면 전체 카드들의 정보에 접근할 수 있다.
[CreateAssetMenu(fileName = "MagicSO", menuName = "Scriptable Object/MagicSO")]
public class MagicSO : ScriptableObject
{
    public MagicResources[] items;
}
