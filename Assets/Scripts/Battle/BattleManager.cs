﻿using System;
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
    public bool extraTurn;//该回合是否是额外回合
    public int tiShuCardCombo;//本回合使用的体术牌数量
    public GameObject enemyPrefab;//敌人预制体
    public List<BattleData> battleDataList = new List<BattleData>();//保存所有战斗场景数据的列表
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
        
        hasCanXin = false;

        if (actionsEndTurn != null)
        {
            foreach (var action in actionsEndTurn)
            {
                action();
            }

            actionsEndTurn = new List<Action>();
        }
        if (Player.Instance.CheckState(Value.ValueType.额外回合))//如果玩家拥有额外回合跳过敌人行动直接开始新的玩家回合
        {
            extraTurn = true;
            TurnStart();
            return;
        }
       
        foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
        {
            enemy.shield = 0;
            enemy.TakeAction();
        }
        

        Invoke(nameof(TurnStart), 1);
        if (!Player.Instance.CheckState(Value.ValueType.保留手牌))
        {
            CardManager.Instance.DropAllCard();

        }
        turnHasEnd = true;
        cardCombo = 0;
        tiShuCardCombo = 0;
    }

    public void TurnStart()
    {
        StateManager.UpdatePlayerState();//对玩家身上的状态进行更新
        StateManager.UpdateEnemiesState();//对敌人身上的状态进行更新
        turnHasEnd = false;
        Player.Instance.InitEnergy();
        if (!extraTurn)//如果是额外回合，则不清除护甲并保留手牌
        {
            Player.Instance.shield = 0;
        }

        extraTurn = false;
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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void BattleStart(BattleData data)//战斗开始
    {
        CardManager.Instance.InitDrawCardList();
        CreateEnemies(data.enemyDataList);
        TurnStart();
    }

    public void CreateEnemies(List<EnemyData> enemyDataList) //创建敌人
    {
        foreach (var enemyData in enemyDataList)
        {
            Enemy newNEnemy = Instantiate(enemyPrefab,enemyData.position,Quaternion.identity).GetComponent<Enemy>();
            newNEnemy.InitEnemy(enemyData);
        }
    }
    void Update() 
    {
        
    }
}
