using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackMagic : MonoBehaviour
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] Sprite skillIcon;

    protected int amount;
    protected float probability;

    protected DeckCard card;
    protected List<BuffDebuffMagic> buffDebuffMagics;

    private void OnEnable()
    {
        skillEffect.SetActive(true);
        // 버프 리스트에 추가
    }

    public Sprite GetSkillIcon()
    {
        return skillIcon;
    }

    public virtual void Delete()
    {
        // 만약 오브젝트 풀링 쓸거면 변수 초기화 여기서 해줘야함
        Managers.Pool.Push(GetComponent<Poolable>());
    }

}
