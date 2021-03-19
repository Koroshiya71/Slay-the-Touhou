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
            XmlAttributeCollection cardAttributes=cardNode.Attributes;
            data.cardID = cardAttributes["CardID"].InnerText;
            switch (cardAttributes["Type"].InnerText)
            {
                case "����":
                    data.type = Card.CardType.Attack;
                    break;
                case "����":
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
