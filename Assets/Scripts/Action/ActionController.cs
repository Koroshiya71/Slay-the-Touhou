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
        Special
    }

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
    }

    //实行行动
    public void TakeAction(EnemyAction a, Enemy thisEnemy)
    {
        switch (a.data.ActID)
        {
            case "0000"://休眠
                break;
            case "0001": //小幽灵攻击
            case "0002": //中幽灵攻击
                Attack(thisEnemy);
                break;
            case "0003"://灵体
            case "0004"://魂体
                Buff(thisEnemy,a);
                break;
            
        }
        
    }

    public void Attack(Enemy enemy) //怪物攻击的通用方法
    {
        Player.Instance.TakeDamage(enemy.actualValue);
        enemy.animController.SetTrigger("Attack");//播放攻击动画

    }
    public void Defend(Enemy enemy) //怪物攻击的通用方法
    {
        enemy.GetShield(enemy.actualValue);

    }

    public void Buff(Enemy enemy, EnemyAction action)
    {
        foreach (var type in action.valueDic.Keys)
        {
            StateManager.AddStateToEnemy(new Value(){type = type,value = action.valueDic[type]},enemy);
        }
    }
}