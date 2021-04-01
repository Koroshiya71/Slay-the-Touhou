using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public static ActionController Instance;
    //行动数据列表
    public List<ActionData> actionDataList = new List<ActionData>();

    public enum ActionType
    {
        Attack,
        Defend,
        Buff,
        DeBuff,
        Special,
        Unknown
    }

    private void Awake()
    {
        Instance = this;
        actionDataList = DataManager.Instance.LoadActionData();
    }


    private void Start()
    {
    }

    //实行行动
    public void TakeAction(EnemyAction a, Enemy thisEnemy)
    {
        switch (a.data.ActID)
        {
            case "0000"://昏迷
                break;
            case "0001": //小幽灵攻击
            case "0002": //中幽灵攻击
            case "0005": //懒惰幽灵攻击
            case "0006": //暴躁妖精攻击1
            case "0007": //暴躁妖精攻击2
            case "0008": //大幽灵攻击1
            case "0009": //大幽灵攻击2
            case "0011": //小妖精攻击
            case "0014": //大妖精攻击
            case "0016": //迷路妖精攻击
                Attack(thisEnemy);
                break;
            case "0003"://灵体
                Buff(thisEnemy,a);
                break;
            case "0004"://懒惰幽灵防御
            case "0010": //大幽灵防御
            case "0013": //小妖精防御
            case "0015": //大妖精防御
                Defend(thisEnemy);
                break;
            case "0012"://小妖精抽牌减少
                DeBuff(thisEnemy,a);
                break;
            case "0017"://迷路妖精逃跑
            case "0018"://迷路妖精晕眩
                Special(thisEnemy, a);
                break;
        }

    }

    public void Special(Enemy enemy,EnemyAction a)
    {
        if (a.valueDic.ContainsKey(Value.ValueType.晕眩))
        {
            for (int i = 0; i < a.valueDic[Value.ValueType.晕眩]; i++)
            {
                foreach (var data in CardManager.Instance.CardDataList)
                {
                    if (data.cardID=="3001")
                    {
                        CardData newData = data;
                        CardManager.Instance.discardList.Add(newData);
                    }
                }
            }
        }
        if (a.valueDic.ContainsKey(Value.ValueType.逃离战斗))
        {
            enemy.EnemyDie();
        }
    }
    public void Attack(Enemy enemy) //怪物攻击的通用方法
    {
        Player.Instance.TakeDamage(enemy.actualValue);
        enemy.animController.SetTrigger("Attack");//播放攻击动画

    }
    public void Defend(Enemy enemy) //怪物攻击的通用方法
    {
        if (enemy.enemyData.ID==6)//大妖精防御
        {
            foreach (var e in EnemyManager.Instance.InGameEnemyList)
            {
                e.GetShield(enemy.actualValue);
            }
        }
        else
            enemy.GetShield(enemy.actualValue);
    }

    public void Buff(Enemy enemy, EnemyAction action)
    {
        foreach (var type in action.valueDic.Keys)
        {
            StateManager.AddStateToEnemy(new Value(){type = type,value = action.valueDic[type]},enemy);
        }
    }

    public void DeBuff(Enemy enemy, EnemyAction action)
    {
        foreach (var type in action.valueDic.Keys)
        {
            StateManager.AddStateToPlayer(new Value() { type = type, value = action.valueDic[type] });
        }
    }
}