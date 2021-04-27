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
    public int exp;//给予的经验值
    public int gold;//给予的金币
    public static EnemyData Clone(EnemyData data)
    {
        EnemyData newData = new EnemyData();
        newData.ID = data.ID;
        newData.Name = data.Name;
        newData.maxHp = data.maxHp;
        newData.initHp = data.maxHp;
        newData.initShield = data.initShield;
        newData.ActionIdList = new List<string>();
        newData.gold = data.gold;
        newData.exp = data.exp;
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
        if (target.valueList.Count>0)
        {
            foreach (var val in target.valueList)
            {
                newData.valueList.Add(val);
            }
        }
        
        newData.spriteID = target.spriteID;
        newData.targetType = target.targetType;
        newData.times = target.times;
        newData.canXinList = new List<CanXin>();
        if (target.canXinList.Count>0)
        {
            foreach (var val in target.canXinList)
            {
                newData.canXinList.Add(val);
            }
        }
        
        newData.keepChangeInBattle= target.keepChangeInBattle;
        newData.comboList = new List<Combo>();
        if (target.comboList.Count>0)
        {
            foreach (var val in target.comboList)
            {
                newData.comboList.Add(val);
            }
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
    public int actProbability;//执行该行动的概率
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
        体术以外禁止,
        体术限制,
        弹幕限制,
        法术限制,
        技能限制,
        防御限制,
        随机限制,
        无何有,
        抽牌,
        击杀回费,
        回血,
        惊吓,
        背水一战,
        起势,
        保留手牌,
        六根清净,
        灵体,
        魂体,
        休眠,
        重伤,
        抽牌减1,
        空无,
        无法使用,
        逃离战斗,
        晕眩,
        盾击,
        保留护甲,
        增幅,
        净化,
        焕发
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
    [Serializable]
    public struct SceneEnemy
    {
        public int ID; //敌人ID
        public Vector3 Pos; //敌人位置
    }
    
    public List<SceneEnemy> EnemyList=new List<SceneEnemy>();
}

[Serializable]
public class SceneData
{
    public SceneManager.SceneType type;//场景类型
    public BattleData battleData;//当前场景的战斗数据
    public int eventID;//事件ID
}

[Serializable]
public struct StateImgData //状态类型预计对应的图片
{
    public Value.ValueType type;
    public Sprite sprite;
}
[Serializable]
public class GameEvent //游戏内的事件
{
    public int eventID;
    public List<string> descriptionList=new List<string>();//事件的描述列表
    public List<string> choiceList = new List<string>();//选择的描述列表
    public List<int> choiceNumList=new List<int>();//事件的每个阶段有几个选择
}

public class RelicData //遗物的数据
{
    public int relicID;
}