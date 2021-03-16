using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool turnHasEnd;
    public void TurnEnd()
    {
        if (turnHasEnd)
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
