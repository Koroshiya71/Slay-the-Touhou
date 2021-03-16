using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public static ActionController Instance;
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

    public void TakeAction(Action a, Enemy thisEnemy)
    {
        switch (a.data.ActID)
        {
            case "0001": //sakuya攻击1
                Player.Instance.TakeDamage(a.valueDic[Value.ValueType.Damage]);
                break;
            case "0002": //sakuya格挡1
                thisEnemy.shield += a.valueDic[Value.ValueType.Shield];
                break;
            case "0003": //sakuya攻击2
                Player.Instance.TakeDamage(a.valueDic[Value.ValueType.Damage]);
                break;
        }
    }
}