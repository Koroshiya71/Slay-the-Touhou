using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool turnHasEnd;
    public List<Action> actionsEndTurn=new List<Action>();//回合结束时触发的效果
    public List<Action> actionsTurnStart=new List<Action>();//回合开始时触发的效果
    public bool hasCanXin;//上回合是否触发过残心
    public int effectTimes;//卡牌效果触发的次数
    public int cardCombo;//本回合使用的卡牌数量
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

        foreach (var state in Player.Instance.stateList)
        {
            if (state.type==Value.ValueType.二刀流)
            {
                state.value -= 1;
            }
        }
        StateManager.CleanPlayerState();//清除Player身上的无效状态
        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
        {
            enemy.shield = 0;
            enemy.TakeAction();
        }
        hasCanXin = false;
        if (actionsEndTurn != null)
        {
            foreach (var action in actionsEndTurn)
            {
                action();
            }

            actionsEndTurn = new List<Action>();
        }

        Invoke(nameof(TurnStart), 1);
        CardManager.Instance.DropAllCard();
        turnHasEnd = true;
        cardCombo = 0;
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

        if (actionsTurnStart!=null)
        {
            foreach (var action in actionsTurnStart)
            {
                action();
            }

            actionsTurnStart = new List<Action>();
        }
        foreach (var cardObject in CardManager.Instance.handCardList)
        {
            Card card=cardObject.GetComponent<Card>();
            card.UpdateCardState();
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
