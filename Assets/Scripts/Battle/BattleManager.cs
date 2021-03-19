using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool turnHasEnd;
    public void TurnEnd()
    {
        if (turnHasEnd)//如果回合已结束在进行运行其他方法时跳过检测
        {
            return;
        }
        if (MenuEventManager.Instance.isPreviewing)//如果正在进行卡牌预览则不进行检测
        {
            return;
        }
        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
        {
            enemy.shield = 0;
            enemy.TakeAction();
        }
        Invoke(nameof(TurnStart), 1);
        CardManager.Instance.DropAllCard();
        turnHasEnd = true;
    }

    public void TurnStart()
    {
        
        turnHasEnd = false;
        Player.Instance.InitEnergy();
        Player.Instance.shield = 0;
        for (int i = 0; i < Player.Instance.drawCardNum; i++)
        {
            CardManager.Instance.DrawCard();
        }

    }
    void Start()
    {
        Instance = this;
        CardManager.Instance.InitDrawCardList();
        TurnStart();
    }

    void Update() 
    {
        
    }
}
