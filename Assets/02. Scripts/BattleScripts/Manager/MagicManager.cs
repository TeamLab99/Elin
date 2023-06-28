/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] Sprite[] effectimages;
    [SerializeField] GameObject[] effect;
    
    Monster monster;
    Player player;
    

    public void SetEntites(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public void UseMagic(int index)
    {
        switch (index)
        {
            case 1: HeadButt(); break;
            case 2: Heal(); break;
            case 3: Defense(); break;
            case 4: Rolling(); break;
            default: break;
        }
    }

    public void Rolling()
    {
        effect[0].GetComponent<SpriteRenderer>().sprite = effectimages[0];
        effect[0].gameObject.SetActive(true);


        if (!BuffManager.Inst.GetisAvoid())
            BuffManager.Inst.AvoidOn();
        else
        {
            BuffManager.Inst.AvoidTimeUpdate();
        }
    }

    public void HeadButt()
    {
        EffectManager.Inst.HeadButtMotion(player.gameObject, 0.3f);
        StartCoroutine(headButt(effect[1],0.5f));
        player.Attack(monster);
    }

    public void Defense()
    {
        effect[0].GetComponent<SpriteRenderer>().sprite = effectimages[1];
        effect[0].gameObject.SetActive(true);
        BuffManager.Inst.DefenseOn();
        player.BuffDef = 5;
    }

    public void Heal()
    {
        StartCoroutine(headButt(effect[2],0.7f));
        player.Heal(5);
    }

    public IEnumerator headButt(GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
*/