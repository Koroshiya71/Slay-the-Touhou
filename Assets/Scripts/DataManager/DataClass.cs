using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public int ID;
    public string Name;//名字
    public int maxHp;//最大生命值
    public int initHp;//初始生命值
    public int initShield;//初始护盾值
    public List<string> ActionIdList = new List<string>();//该Enemy所有行为的ID
    public Vector3 position;//敌人的生成位置

    public static EnemyData Clone(EnemyData data)
    {
        EnemyData newData = new EnemyData();
        newData.ID = data.ID;
        newData.Name = data.Name;
        newData.maxHp = data.maxHp;
        newData.initHp = data.maxHp;
        newData.initShield = data.initShield;
        newData.ActionIdList = new List<string>();
        newData.position = data.position;
        foreach (var action in data.ActionIdList)
        {
         newData.ActionIdList.Add(action);   
        }
        return newData;
    }
}
[Serializable]
public class CardData
{
    public enum TargetType//目标类型
    {
        单体敌人,
        自身,
        全部敌人,
        随机敌人
    }
    public Card.CardType type;//卡牌类型
    public string name;//卡牌名称
    public string cardID;//卡牌ID
    public int cost;//卡牌消耗
    public string des;//卡牌描述
    public List<Value> valueList;//效果列表
    public int spriteID;//卡图ID
    public TargetType targetType;//目标类型
    public int times;//卡牌效果触发的次数
    public List<CanXin> canXinList;//残心列表
    public bool keepChangeInBattle;//在战斗中保留对其的更改
    public List<Combo> comboList;//连斩列表
    public CardRare rare;
    public enum CardRare//稀有度
    {
        Normal,//普通
        Rare,//稀有
        Epic//史诗
    }
    public static CardData Clone(CardData target)
    {
        CardData newData = new CardData();
        newData.type = target.type;
        newData.name = target.name;
        newData.cardID = target.cardID;
        newData.cost = target.cost;
        newData.des = target.des;
        newData.valueList = new List<Value>();
        newData.rare = target.rare;
        foreach (var val in target.valueList)
        {
            newData.valueList.Add(val);
        }
        newData.spriteID = target.spriteID;
        newData.targetType = target.targetType;
        newData.times = target.times;
        newData.canXinList = new List<CanXin>();
        foreach (var val in target.canXinList)
        {
            newData.canXinList.Add(val);
        }
        newData.keepChangeInBattle= target.keepChangeInBattle;
        newData.comboList = new List<Combo>();
        foreach (var val in target.comboList)
        {
            newData.comboList.Add(val);
        }
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
        回费,
        流转,
        额外回合,
        体术限制,
        无何有,
        抽牌,
        击杀回费,
        回血,
        惊吓,
        背水一战,
        起势,
        保留手牌,
        六根清净
    }
    public ValueType type;
    public int value;

    
}
[Serializable]
public class CanXin//残心
{
    public Value CanXinValue;
    public bool IsTurnEnd;//是否是回合结束触发
}
[Serializable]
public class Combo//连斩
{
    public Value comboValue;
    public int comboNum;//连斩数
}

[Serializable]
public class BattleData
{
    public List<int> enemyIdList;//敌人的ID列表
}

[Serializable]
public class SceneData
{
    public SceneManager.SceneType type;//场景类型
    public BattleData battleData;//当前场景的战斗数据
}

[Serializable]
public struct StateImgData //状态类型预计对应的图片
{
    public Value.ValueType type;
    public Sprite sprite;
}