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
            XmlAttributeCollection cardAttributes=cardNode.Attributes;
            data.cardID = cardAttributes["CardID"].InnerText;
            switch (cardAttributes["Type"].InnerText)
            {
                case "攻击":
                    data.type = Card.CardType.Attack;
                    break;
                case "技能":
                    data.type = Card.CardType.Skill;
                    break;
            }
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
