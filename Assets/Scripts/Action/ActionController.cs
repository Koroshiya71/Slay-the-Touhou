using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
            case "0000": //昏迷
            case "0023": //观察
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
            case "0019": //大蝴蝶攻击
            case "0021": //大蝴蝶盾击
                Attack(thisEnemy,a);
                break;
            case "0003": //灵体
                Buff(thisEnemy, a);
                break;
            case "0004": //懒惰幽灵防御
            case "0010": //大幽灵防御
            case "0013": //小妖精防御
            case "0015": //大妖精防御
            case "0022": //大蝴蝶防御
                Defend(thisEnemy);
                break;
            case "0012": //小妖精抽牌减少
            case "0020": //大蝴蝶随机限制
                DeBuff(thisEnemy, a);
                break;
            case "0017": //迷路妖精逃跑
            case "0018": //迷路妖精晕眩
                Special(thisEnemy, a);
                break;
        }
    }

    public void Special(Enemy enemy, EnemyAction a)
    {
        if (a.valueDic.ContainsKey(Value.ValueType.晕眩))
            for (var i = 0; i < a.valueDic[Value.ValueType.晕眩]; i++)
                foreach (var data in CardManager.Instance.CardDataList)
                    if (data.cardID == "3001")
                    {
                        var newData = data;
                        CardManager.Instance.discardList.Add(newData);
                    }

        if (a.valueDic.ContainsKey(Value.ValueType.逃离战斗)) enemy.EnemyDie();

    }

    public void Attack(Enemy enemy,EnemyAction a) //怪物攻击的通用方法
    {
        if (a.valueDic.ContainsKey(Value.ValueType.护甲)) enemy.GetShield(a.valueDic[Value.ValueType.护甲]);
        enemy.UpdateUIState();
        Player.Instance.TakeDamage(enemy.actualValue);
        if (a.valueDic.ContainsKey(Value.ValueType.惊吓))
            StateManager.AddStateToPlayer(new Value()
            {
                type = Value.ValueType.惊吓,
                value = a.valueDic[Value.ValueType.惊吓]
            });

        enemy.animController.SetTrigger("Attack"); //播放攻击动画
    }

    public void Defend(Enemy enemy) //怪物攻击的通用方法
    {
        if (enemy.enemyData.ID == 6) //大妖精防御
            foreach (var e in EnemyManager.Instance.InGameEnemyList)
                e.GetShield(enemy.actualValue);
        else
            enemy.GetShield(enemy.actualValue);
        if (enemy.currentEnemyAction.valueDic.ContainsKey(Value.ValueType.保留护甲))
        {
            StateManager.AddStateToEnemy(new Value() { type = Value.ValueType.保留护甲, value = enemy.currentEnemyAction.valueDic[Value.ValueType.保留护甲] }, enemy);
        }
    }

    public void Buff(Enemy enemy, EnemyAction action)
    {
        foreach (var type in action.valueDic.Keys)
            StateManager.AddStateToEnemy(new Value() {type = type, value = action.valueDic[type]}, enemy);
    }

    public void DeBuff(Enemy enemy, EnemyAction action)
    {
        foreach (var type in action.valueDic.Keys)
            if (type == Value.ValueType.随机限制)
            {
                var n = Random.Range(0, 5);
                switch (n)
                {
                    case 0:
                        StateManager.AddStateToPlayer(new Value()
                        {
                            type = Value.ValueType.体术限制, 
                            value = action.valueDic[Value.ValueType.随机限制]
                        });
                        continue;
                    case 1:
                        StateManager.AddStateToPlayer(new Value()
                        {
                            type = Value.ValueType.技能限制,
                            value = action.valueDic[Value.ValueType.随机限制]
                        });
                        continue;

                    case 2:
                        StateManager.AddStateToPlayer(new Value()
                        {
                            type = Value.ValueType.法术限制,
                            value = action.valueDic[Value.ValueType.随机限制]
                        });
                        continue;

                    case 3:
                        StateManager.AddStateToPlayer(new Value()
                        {
                            type = Value.ValueType.弹幕限制,
                            value = action.valueDic[Value.ValueType.随机限制]
                        });
                        continue;

                    case 4:
                        StateManager.AddStateToPlayer(new Value()
                        {
                            type = Value.ValueType.防御限制,
                            value = action.valueDic[Value.ValueType.随机限制]
                        });
                        continue;

                }
                StateManager.AddStateToPlayer(new Value() { type = type, value = action.valueDic[type] });
            }
    }
}