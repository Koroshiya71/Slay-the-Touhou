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
        Defend
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
            case "0001": //sakuya攻击1
                Player.Instance.TakeDamage(thisEnemy.actualValue);
                break;
            case "0002": //sakuya格挡1
                thisEnemy.shield += a.valueDic[Value.ValueType.护甲];
                break;
            case "0003": //sakuya攻击2
                Player.Instance.TakeDamage(thisEnemy.actualValue);
                break;
        }

        if (a.data.Type==ActionType.Attack)
        {
            thisEnemy.animController.SetTrigger("Attack");//播放攻击动画
        }
    }
}