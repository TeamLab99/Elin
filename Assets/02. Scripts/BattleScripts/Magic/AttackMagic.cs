using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackMagic : MonoBehaviour
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] Sprite skillIcon;

    protected DeckCard card;

    protected float percent;
    protected float probability;

    protected int amount;

    protected bool isEnd = false;

    // 공격 추가 버프/디버프 저장?
    protected BattleBuffManager buffManager;

    public abstract float CalculateAttackValue(float value);

    public virtual void Delete()
    {
        // 만약 오브젝트 풀링 쓸거면 변수 초기화 여기서 해줘야함
        Managers.Pool.Push(GetComponent<Poolable>());
    }

    public void ConnectBuffManager(BattleBuffManager buffManager, SkillIcon icon)
    {
        this.buffManager = buffManager;
    }

    void OnEnable()
    {
        skillEffect.SetActive(true);
    }
}
