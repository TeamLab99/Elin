using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffDebuffMagic : MonoBehaviour
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] Sprite skilIcon;

    protected DeckCard card;
    protected BattleBuffManager buffManager;
    protected WaitForSeconds oneSec = new WaitForSeconds(1f);
    protected SkillIcon icon;

    protected bool isEnd = false;
    protected bool isTimerStop = false;

    protected float percent;
    protected float probability;
    protected float maintime;
    protected float time;

    protected int amount;

    public abstract IEnumerator Timer();

    private void Start()
    {
        BattleCardManager.EffectPlayBack += TimerControl;
    }

    private void OnEnable()
    {
        time = maintime;
        StartCoroutine(Timer());
        // 이펙트 재생
        skillEffect.SetActive(true);
        // 버프 리스트 및 아이콘 추가 -> 바깥에서 함
    }

    private void OnDestroy()
    {
        BattleCardManager.EffectPlayBack -= TimerControl;
    }

    public virtual void Delete()
    {
        // 만약 오브젝트 풀링 쓸거면 변수 초기화 여기서 해줘야함
        Managers.Pool.Push(GetComponent<Poolable>());
    }

    public void ChangeIcon(SkillIcon icon)
    {
        this.icon = icon;
        IconInit();
    }

    public void TimeUpdate()
    {
        time = maintime;
        icon.countText.text = time.ToString();
        // 이펙트 재생 OnAwake
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ConnectBuffManager(BattleBuffManager buffManager, SkillIcon icon)
    {
        this.buffManager = buffManager;
        this.icon = icon;
        IconInit();
    }

    void TimerControl(bool active)
    {
        isTimerStop = active;
    }

    void IconInit()
    {
        icon.countText.text = time.ToString();
        icon.IconImage.sprite = skilIcon;
        icon.isFull = true;
        icon.buff = this;
    }
}