using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string Name;//名字
    public int maxHp;//最大生命值
    public int initHp;//初始生命值
    public int initShield;//初始护盾值
    public List<string> ActionIdList = new List<string>();//该Enemy所有行为的ID
}
[Serializable]
public class CardData
{
    public Card.CardType type;//卡牌类型
    public string name;//卡牌名称
    public string cardID;//卡牌ID
    public int cost;//卡牌消耗
    public string des;//卡牌描述
    public List<Value> valueList;//效果列表
    public int spriteID;//卡图ID
    public bool needTarget;//是否需要目标
    public int times;//卡牌效果触发的次数
    public List<CanXin> canXinList;//残心列表
    public bool keepChangeInBattle;//在战斗中保留对其的更改

    public static CardData Clone(CardData target)
    {
        CardData newData = new CardData();
        newData.type = target.type;
        newData.name = target.name;
        newData.cardID = target.cardID;
        newData.cost = target.cost;
        newData.des = target.des;
        newData.valueList = target.valueList;
        newData.spriteID = target.spriteID;
        newData.needTarget = target.needTarget;
        newData.times = target.times;
        newData.canXinList = target.canXinList;
        newData.keepChangeInBattle= target.keepChangeInBattle;
        return newData;

    }
}
[Serializable]
public class ActionData
{
    public ActionController.ActionType Type;
    public string Name;
    public string ActID;
    public List<Value> valueList;//数值列表
}
[Serializable]
public class Value
{
    public enum ValueType//数值类型的枚举类
    {
        伤害,
        护甲,
        二刀流,
        回费
    }
    public ValueType type;
    public int value;

    
}
[Serializable]
public class CanXin
{
    public Value CanXinValue;
    public bool IsTurnEnd;//是否是回合结束触发
}