using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    //卡牌数据文件路径
    public string cardDataPath;
    public string actionDataPath;
    public string enemyDataPath;
    private void Awake()
    {
        Instance = this;
        cardDataPath = Application.dataPath+"/StreamingAssets/Data/CardData.xml";
        actionDataPath = Application.dataPath + "/StreamingAssets/Data/ActionData.xml";
        enemyDataPath = Application.dataPath + "/StreamingAssets/Data/EnemyData.xml";

    }

    public  List<CardData> LoadCardData()//从xml数据文件中读取卡牌数据列表
    {
        var cardDataList = new List<CardData>();

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(cardDataPath);//读取数据文件

        var cardListNode = xmlDoc.FirstChild.FirstChild;
        foreach (XmlNode cardNode in cardListNode)//根据表中数据添加卡牌数据
        {
            CardData data = new CardData();
            data.valueList = new List<Value>();
            data.canXinList= new List<CanXin>();
            data.comboList = new List<Combo>();
            XmlAttributeCollection cardAttributes =cardNode.Attributes;
            data.cardID = cardAttributes["CardID"].InnerText;
            switch (cardAttributes["Rare"].InnerText)
            {

                case "Normal":
                    data.rare = CardData.CardRare.Normal;

                    break;
                case "Rare":
                    data.rare = CardData.CardRare.Rare;
                    break;
                case "Epic":
                    data.rare = CardData.CardRare.Epic;
                    break;
            }
            switch (cardAttributes["Type"].InnerText)
            {
                case "体术":
                    data.type = Card.CardType.体术;
                    break;
                case "弹幕":
                    data.type = Card.CardType.弹幕;
                    break;
                case "防御":
                    data.type = Card.CardType.防御;
                    break;
                case "技能":
                    data.type = Card.CardType.技能;
                    break;
                case "法术":
                    data.type = Card.CardType.法术;
                    break;
                case "符卡":
                    data.type = Card.CardType.符卡;
                    break;
                case "状态":
                    data.type = Card.CardType.状态;
                    break;
            }

            data.name = cardAttributes["Name"].InnerText;
            data.cost = int.Parse(cardAttributes["Cost"].InnerText);
            data.spriteID = int.Parse(cardAttributes["SpriteID"].InnerText);
            switch (cardAttributes["TargetType"].InnerText)
            {
                case "单体敌人":
                    data.targetType = CardData.TargetType.单体敌人;
                    break;
                case "自身":
                    data.targetType = CardData.TargetType.自身;

                    break;
                case "全部敌人":
                    data.targetType = CardData.TargetType.全部敌人;
                    break;
                case "随机敌人":
                    data.targetType = CardData.TargetType.随机敌人;
                    break;
            }

            data.times = Int32.Parse(cardAttributes["Times"].InnerText);

            XmlNode ValuesNode = cardNode.FirstChild;
            foreach (XmlNode valueNode in ValuesNode)
            {
                Value newValue = new Value();
                var valueAttributes = valueNode.Attributes;
                switch (valueAttributes["Type"].InnerText)
                {
                    case "伤害":
                        newValue.type = Value.ValueType.伤害;
                        break;
                    case "护甲":
                        newValue.type = Value.ValueType.护甲;
                        break;
                    case "二刀流":
                        newValue.type = Value.ValueType.二刀流;
                        break;
                    case "回费":
                        newValue.type = Value.ValueType.回费;
                        break;
                    case "流转":
                        newValue.type = Value.ValueType.流转;
                        break;
                    case "额外回合":
                        newValue.type = Value.ValueType.额外回合;
                        break;
                    case "无何有":
                        newValue.type = Value.ValueType.无何有;
                        break;
                    case "体术以外禁止":
                        newValue.type = Value.ValueType.体术以外禁止;
                        break;
                    case "抽牌":
                        newValue.type = Value.ValueType.抽牌;
                        break;
                    case "击杀回费":
                        newValue.type = Value.ValueType.击杀回费;
                        break;
                    case "回血":
                        newValue.type = Value.ValueType.回血;
                        break;
                    case "惊吓":
                        newValue.type = Value.ValueType.惊吓;
                        break;
                    case "背水一战":
                        newValue.type = Value.ValueType.背水一战;
                        break;
                    case "起势":
                        newValue.type = Value.ValueType.起势;
                        break;
                    case "保留手牌":
                        newValue.type = Value.ValueType.保留手牌;
                        break;
                    case "六根清净":
                        newValue.type = Value.ValueType.六根清净;
                        break;
                    case "无法使用":
                        newValue.type = Value.ValueType.无法使用;
                        break;
                    case "空无":
                        newValue.type = Value.ValueType.空无;
                        break;
                }

                newValue.value = Int32.Parse(valueAttributes["Value"].InnerText);
                data.valueList.Add(newValue);
            }

            XmlNode CanXinListNode = cardNode["CanXinList"];
            if (CanXinListNode!=null)//如果有残心列表的话
            {
                foreach (XmlNode CanXinNode in CanXinListNode)
                {
                    CanXin newCanXin = new CanXin();
                    newCanXin.CanXinValue = new Value();
                    var canXinAttributes = CanXinNode.Attributes;
                    switch (canXinAttributes["Type"].InnerText)
                    {
                        case "伤害":
                            newCanXin.CanXinValue.type = Value.ValueType.伤害;
                            break;
                        case "护甲":
                            newCanXin.CanXinValue.type = Value.ValueType.护甲;
                            break;
                        case "二刀流":
                            newCanXin.CanXinValue.type = Value.ValueType.二刀流;
                            break;
                        case "回费":
                            newCanXin.CanXinValue.type = Value.ValueType.回费;
                            break;
                        case "回血":
                            newCanXin.CanXinValue.type = Value.ValueType.回血;
                            break;
                        case "惊吓":
                            newCanXin.CanXinValue.type = Value.ValueType.惊吓;
                            break;
                    }
                    newCanXin.CanXinValue.value = Int32.Parse(canXinAttributes["Value"].InnerText);
                    newCanXin.IsTurnEnd = canXinAttributes["Time"].InnerText == "1";
                    data.canXinList.Add(newCanXin);

                }
            }
            XmlNode ComboListNode = cardNode["ComboList"];
            if (ComboListNode != null)//如果有连斩列表的话
            {
                foreach (XmlNode comboNode in ComboListNode)
                {
                    Combo newCombo = new Combo();
                    newCombo.comboValue = new Value();
                    var comboAttributes = comboNode.Attributes;
                    switch (comboAttributes["Type"].InnerText)
                    {
                        case "伤害":
                            newCombo.comboValue.type = Value.ValueType.伤害;
                            break;
                        case "护甲":
                            newCombo.comboValue.type = Value.ValueType.护甲;
                            break;
                        case "二刀流":
                            newCombo.comboValue.type = Value.ValueType.二刀流;
                            break;
                        case "回费":
                            newCombo.comboValue.type = Value.ValueType.回费;
                            break;
                    }
                    newCombo.comboValue.value = Int32.Parse(comboAttributes["Value"].InnerText);
                    newCombo.comboNum= Int32.Parse(comboAttributes["Combo"].InnerText);
                    data.comboList.Add(newCombo);

                }
            }
            cardDataList.Add(data);
        }
        return cardDataList;
    }

    public List<ActionData> LoadActionData() //从xml数据文件中读取敌人行为数据列表
    {
        List<ActionData> actionDataList = new List<ActionData>();
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(actionDataPath);//读取数据文件

        var actionListNode = xmlDoc.FirstChild.FirstChild;
        foreach (XmlNode actionNode in actionListNode)
        {
            ActionData newData = new ActionData();
            newData.valueList = new List<Value>();
            XmlAttributeCollection actionAttributes = actionNode.Attributes;
            newData.ActID = actionAttributes["ActID"].InnerText;
            newData.Name = actionAttributes["Name"].InnerText;
            newData.actProbability = Int32.Parse(actionAttributes["Probability"].InnerText);
            switch (actionAttributes["Type"].InnerText)
            {
                case "Attack":
                    newData.Type = ActionController.ActionType.Attack;
                    break;
                case "Defend":
                    newData.Type = ActionController.ActionType.Defend;
                    break;
                case "Buff":
                    newData.Type = ActionController.ActionType.Buff;
                    break;
                case "Special":
                    newData.Type = ActionController.ActionType.Special;
                    break;
                case "DeBuff":
                    newData.Type = ActionController.ActionType.DeBuff;
                    break;
            }
            XmlNode ValuesNode = actionNode.FirstChild;
            foreach (XmlNode valueNode in ValuesNode)
            {
                Value newValue = new Value();
                var valueAttributes = valueNode.Attributes;
                switch (valueAttributes["Type"].InnerText)
                {
                    case "伤害":
                        newValue.type = Value.ValueType.伤害;
                        break;
                    case "护甲":
                        newValue.type = Value.ValueType.护甲;
                        break;
                    case "二刀流":
                        newValue.type = Value.ValueType.二刀流;
                        break;
                    case "回费":
                        newValue.type = Value.ValueType.回费;
                        break;
                    case "流转":
                        newValue.type = Value.ValueType.流转;
                        break;
                    case "额外回合":
                        newValue.type = Value.ValueType.额外回合;
                        break;
                    case "无何有":
                        newValue.type = Value.ValueType.无何有;
                        break;
                    case "体术以外禁止":
                        newValue.type = Value.ValueType.体术以外禁止;
                        break;
                    case "法术限制":
                        newValue.type = Value.ValueType.法术限制;
                        break;
                    case "技能限制":
                        newValue.type = Value.ValueType.技能限制;
                        break;
                    case "弹幕限制":
                        newValue.type = Value.ValueType.弹幕限制;
                        break;
                    case "防御限制":
                        newValue.type = Value.ValueType.防御限制;
                        break;
                    case "随机限制":
                        newValue.type = Value.ValueType.随机限制;
                        break;
                    case "抽牌":
                        newValue.type = Value.ValueType.抽牌;
                        break;
                    case "击杀回费":
                        newValue.type = Value.ValueType.击杀回费;
                        break;
                    case "回血":
                        newValue.type = Value.ValueType.回血;
                        break;
                    case "惊吓":
                        newValue.type = Value.ValueType.惊吓;
                        break;
                    case "背水一战":
                        newValue.type = Value.ValueType.背水一战;
                        break;
                    case "起势":
                        newValue.type = Value.ValueType.起势;
                        break;
                    case "保留手牌":
                        newValue.type = Value.ValueType.保留手牌;
                        break;
                    case "六根清净":
                        newValue.type = Value.ValueType.六根清净;
                        break;
                    case "休眠":
                        newValue.type = Value.ValueType.休眠;
                        break;
                    case "抽牌减1":
                        newValue.type = Value.ValueType.抽牌减1;
                        break;
                    case "逃离战斗":
                        newValue.type = Value.ValueType.逃离战斗;
                        break;
                    case "晕眩":
                        newValue.type = Value.ValueType.晕眩;
                        break;
                    case "盾击":
                        newValue.type = Value.ValueType.盾击;
                        break;
                    case "增幅":
                        newValue.type = Value.ValueType.增幅;
                        break;
                    case "保留护甲":
                        newValue.type = Value.ValueType.保留护甲;
                        break;
                    case "净化":
                        newValue.type = Value.ValueType.净化;
                        break;
                }

                newValue.value = Int32.Parse(valueAttributes["Value"].InnerText);
                newData.valueList.Add(newValue);
            }
            actionDataList.Add(newData);
        }
        return actionDataList;
    }

    public List<EnemyData> LoadEnemyData()//从xml数据文件中读取敌人行为数据列表
    {
        List<EnemyData> enemyDataList = new List<EnemyData>();
        var cardDataList = new List<CardData>();

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(enemyDataPath);//读取数据文件

        var enemyListNode = xmlDoc.FirstChild.FirstChild;

        foreach (XmlNode enemyNode in enemyListNode)
        {
            EnemyData newData = new EnemyData();
            newData.ActionIdList = new List<string>();
            XmlAttributeCollection enemyAttributes = enemyNode.Attributes;
            newData.ID = Int32.Parse(enemyAttributes["EnemyID"].InnerText);
            newData.Name = enemyAttributes["Name"].InnerText;
            newData.maxHp = Int32.Parse(enemyAttributes["MaxHp"].InnerText);
            newData.exp = Int32.Parse(enemyAttributes["Exp"].InnerText);
            newData.gold = Int32.Parse(enemyAttributes["Gold"].InnerText);

            XmlNode actionListNode = enemyNode.FirstChild;
            foreach (XmlNode actionIDNode in actionListNode)
            {
                string actionID = actionIDNode.Attributes["ID"].InnerText;
                newData.ActionIdList.Add(actionID);
            }
            enemyDataList.Add(newData);
        }
        return enemyDataList;
    }
    void Start()
    {
    }

    void Update()
    {
        
    }
}
