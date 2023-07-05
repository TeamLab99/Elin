using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffUI : Singleton<OnOffUI>
{ 
    [SerializeField] GameObject statObject;
    [SerializeField] GameObject invenObject;
    public GameObject endingObject;
    public bool onBattlePage;
    bool statActivate = false;
    bool invenActivate = false;

    private void Start()
    {
        BattleGameManager.PlatformUIControlForBattle += SetBool;
        BattleGameManager.PlatformUIControlForDialouge += SetBool;
    }

    private void OnDestroy()
    {
        BattleGameManager.PlatformUIControlForBattle -= SetBool;
        BattleGameManager.PlatformUIControlForDialouge -= SetBool;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !onBattlePage)
        {
            statActivate = !statActivate;
            statObject.SetActive(statActivate);
        }

        if (Input.GetKeyDown(KeyCode.I) && !onBattlePage)
        {
            invenActivate = !invenActivate;
            invenObject.SetActive(invenActivate);
        }
    }

    void SetBool()
    {
        if (onBattlePage)
            onBattlePage = false;
        else
        {
            onBattlePage = true;
            invenObject.SetActive(false);
            statObject.SetActive(false);
        }
    }
}
