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
    public string cardDataPath = "Assets/Data/CardData.xml";

    private void Awake()
    {
        Instance = this;
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

            XmlAttributeCollection cardAttributes =cardNode.Attributes;
            data.cardID = cardAttributes["CardID"].InnerText;
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
            }

            data.name = cardAttributes["Name"].InnerText;
            data.cost = int.Parse(cardAttributes["Cost"].InnerText);
            data.spriteID = int.Parse(cardAttributes["SpriteID"].InnerText);
            if (cardAttributes["NeedTarget"].InnerText=="1")
            {
                data.needTarget = true;
            }
            else
            {
                data.needTarget = false;
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
                }

                newValue.value = Int32.Parse(valueAttributes["Value"].InnerText);
                data.valueList.Add(newValue);
            }
            XmlNode CanXinListNode = cardNode.LastChild;
            if (CanXinListNode!=cardNode.FirstChild)//如果有残心列表的话
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
                    }
                    newCanXin.CanXinValue.value = Int32.Parse(canXinAttributes["Value"].InnerText);
                    newCanXin.IsTurnEnd = canXinAttributes["Time"].InnerText == "1";
                    data.canXinList.Add(newCanXin);

                }
            }
            Debug.Log(data.cardID);
            cardDataList.Add(data);
        }
        return cardDataList;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
