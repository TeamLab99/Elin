using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드 가지고 있는 속성들의 정보를 담은 스크립트
/// </summary>
[System.Serializable]
public class Item
{
    public string name;
    public int attack;
    public int health;
    public Sprite sprite; // 이미지
    public float percent; // 확률
    public string type; // 카테고리
    public int id; // 카드 아이디
    public int cost;
}


// Items List를 인스펙터창에서 접근할 수 있는 파일로 만들었다.
// 이 리스트를 접근하면 전체 카드들의 정보에 접근할 수 있다.
[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}
