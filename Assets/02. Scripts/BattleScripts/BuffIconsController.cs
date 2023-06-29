using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIcon
{
    public SkillIcon(Image image, TMP_Text count, TMP_Text amount)
    {
        IconImage = image;
        countText = count;
        amountText = amount;
    }

    public Image IconImage;
    public TMP_Text countText;
    public TMP_Text amountText;
    public bool isFull = false;
    public BuffDebuffMagic buff;
}

public class BuffIconsController : Singleton<BuffIconsController>
{
    [SerializeField] GameObject[] playerBuffObjects;
    [SerializeField] GameObject[] monsterBuffObjects;

    List<SkillIcon> playerIconsList = new List<SkillIcon>();
    List<SkillIcon> monsterIconsList = new List<SkillIcon>();

    private void Start()
    {
        Initializing();
    }

    void Initializing()
    {
        for (int i = 0; i < playerBuffObjects.Length; i++)
        {
            var image = playerBuffObjects[i].transform.GetChild(0).GetComponent<Image>();
            var counter = playerBuffObjects[i].transform.GetChild(1).GetComponent<TMP_Text>();
            var amount = playerBuffObjects[i].transform.GetChild(2).GetComponent<TMP_Text>();

            playerIconsList.Add(new SkillIcon(image, counter, amount));

            var mobImage = playerBuffObjects[i].transform.GetChild(0).GetComponent<Image>();
            var mobCounter = playerBuffObjects[i].transform.GetChild(1).GetComponent<TMP_Text>();
            var mobAmount = playerBuffObjects[i].transform.GetChild(2).GetComponent<TMP_Text>();

            monsterIconsList.Add(new SkillIcon(mobImage, mobCounter, mobAmount));
        }
    }

    public SkillIcon GetBuffIconInfo(bool isPlayer)
    {
        var iconList = isPlayer ? playerIconsList : monsterIconsList;

        return iconList.Find(x => x.isFull == false);
    }

    public void DeleteIconInfo(SkillIcon icon)
    {
        // 데이터 초기화
        icon.IconImage.sprite = null;
        icon.countText.text = "";
        icon.amountText.text = "";
        icon.isFull = false;

        // 누구의 버프 리스트에 있었는가 확인
        if (playerIconsList.Contains(icon))
        {
            if (playerIconsList.Exists(x => x.isFull == true))
                CheckNextBuff(playerIconsList, icon);
            else
                return;
        }
        else
        {
            if (monsterIconsList.Exists(x => x.isFull == true))
                CheckNextBuff(monsterIconsList, icon);
            else
                return;
        }
    }

    void CheckNextBuff(List<SkillIcon> icons, SkillIcon target)
    {
        // 초기화된 버프 뒤에 있는 버프들 중 가장 첫번째의 활성화가 되어있는 요소의 인덱스

        // 모든 활성화 되어있는 것들 중에 찾는 것이므로 바꿔야함. 맨 앞에께 true면 그거 가져옴 뒤에거가 필요
        var activationIndex = icons.FindIndex(x => x.isFull == true);
        var targetIndex = icons.FindIndex(x => x == target);

        if (activationIndex > targetIndex)
        {
            for (int i = activationIndex; i < icons.Count; i++)
            {
                if (icons[i].isFull == true)
                {
                    icons[i].buff.ChangeIcon(icons[i - 1]);

                    icons[i].IconImage.sprite = null;
                    icons[i].countText.text = "";
                    icons[i].amountText.text = "";
                    icons[i].isFull = false;
                    icons[i].buff = null;
                }
            }
        }
        else
        {
            return;
        }
    }
}