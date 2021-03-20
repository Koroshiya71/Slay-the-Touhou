using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    //���������ļ�·��
    public string cardDataPath = "Assets/Data/CardData.xml";

    private void Awake()
    {
        Instance = this;
    }

    public  List<CardData> LoadCardData()//��xml�����ļ��ж�ȡ���������б�
    {
        var cardDataList = new List<CardData>();

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(cardDataPath);//��ȡ�����ļ�
        var cardListNode = xmlDoc.FirstChild.FirstChild;
        foreach (XmlNode cardNode in cardListNode)//���ݱ���������ӿ�������
        {
            CardData data = new CardData();
            data.valueList = new List<Value>();
            XmlAttributeCollection cardAttributes=cardNode.Attributes;
            data.cardID = cardAttributes["CardID"].InnerText;
            switch (cardAttributes["Type"].InnerText)
            {
                case "����":
                    data.type = Card.CardType.����;
                    break;
                case "��Ļ":
                    data.type = Card.CardType.��Ļ;
                    break;
                case "����":
                    data.type = Card.CardType.����;
                    break;
                case "����":
                    data.type = Card.CardType.����;
                    break;
                case "����":
                    data.type = Card.CardType.����;
                    break;
                case "����":
                    data.type = Card.CardType.����;
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
                    case "�˺�":
                        newValue.type = Value.ValueType.�˺�;
                        break;
                    case "����":
                        newValue.type = Value.ValueType.����;
                        break;
                }

                newValue.value = Int32.Parse(valueAttributes["Value"].InnerText);
                data.valueList.Add(newValue);
            }
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
