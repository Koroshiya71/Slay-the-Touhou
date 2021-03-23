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
    public string cardDataPath;

    private void Awake()
    {
        Instance = this;
        cardDataPath = Application.dataPath+"/StreamingAssets/Data/CardData.xml";
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
            data.canXinList= new List<CanXin>();
            data.comboList = new List<Combo>();
            XmlAttributeCollection cardAttributes =cardNode.Attributes;
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
            switch (cardAttributes["TargetType"].InnerText)
            {
                case "�������":
                    data.targetType = CardData.TargetType.�������;
                    break;
                case "����":
                    data.targetType = CardData.TargetType.����;

                    break;
                case "ȫ������":
                    data.targetType = CardData.TargetType.ȫ������;
                    break;
                case "�������":
                    data.targetType = CardData.TargetType.�������;
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
                    case "�˺�":
                        newValue.type = Value.ValueType.�˺�;
                        break;
                    case "����":
                        newValue.type = Value.ValueType.����;
                        break;
                    case "������":
                        newValue.type = Value.ValueType.������;
                        break;
                    case "�ط�":
                        newValue.type = Value.ValueType.�ط�;
                        break;
                    case "��ת":
                        newValue.type = Value.ValueType.��ת;
                        break;
                    case "����غ�":
                        newValue.type = Value.ValueType.����غ�;
                        break;
                    case "�޺���":
                        newValue.type = Value.ValueType.�޺���;
                        break;
                    case "��������":
                        newValue.type = Value.ValueType.��������;
                        break;
                    case "����":
                        newValue.type = Value.ValueType.����;
                        break;
                    case "��ɱ�ط�":
                        newValue.type = Value.ValueType.��ɱ�ط�;
                        break;
                    case "��Ѫ":
                        newValue.type = Value.ValueType.��Ѫ;
                        break;
                    case "����":
                        newValue.type = Value.ValueType.����;
                        break;
                    case "��ˮһս":
                        newValue.type = Value.ValueType.��ˮһս;
                        break;
                }

                newValue.value = Int32.Parse(valueAttributes["Value"].InnerText);
                data.valueList.Add(newValue);
            }

            XmlNode CanXinListNode = cardNode["CanXinList"];
            if (CanXinListNode!=null)//����в����б�Ļ�
            {
                foreach (XmlNode CanXinNode in CanXinListNode)
                {
                    CanXin newCanXin = new CanXin();
                    newCanXin.CanXinValue = new Value();
                    var canXinAttributes = CanXinNode.Attributes;
                    switch (canXinAttributes["Type"].InnerText)
                    {
                        case "�˺�":
                            newCanXin.CanXinValue.type = Value.ValueType.�˺�;
                            break;
                        case "����":
                            newCanXin.CanXinValue.type = Value.ValueType.����;
                            break;
                        case "������":
                            newCanXin.CanXinValue.type = Value.ValueType.������;
                            break;
                        case "�ط�":
                            newCanXin.CanXinValue.type = Value.ValueType.�ط�;
                            break;
                        case "��Ѫ":
                            newCanXin.CanXinValue.type = Value.ValueType.��Ѫ;
                            break;
                        case "����":
                            newCanXin.CanXinValue.type = Value.ValueType.����;
                            break;
                    }
                    newCanXin.CanXinValue.value = Int32.Parse(canXinAttributes["Value"].InnerText);
                    newCanXin.IsTurnEnd = canXinAttributes["Time"].InnerText == "1";
                    data.canXinList.Add(newCanXin);

                }
            }
            XmlNode ComboListNode = cardNode["ComboList"];
            if (ComboListNode != null)//�������ն�б�Ļ�
            {
                foreach (XmlNode comboNode in ComboListNode)
                {
                    Combo newCombo = new Combo();
                    newCombo.comboValue = new Value();
                    var comboAttributes = comboNode.Attributes;
                    switch (comboAttributes["Type"].InnerText)
                    {
                        case "�˺�":
                            newCombo.comboValue.type = Value.ValueType.�˺�;
                            break;
                        case "����":
                            newCombo.comboValue.type = Value.ValueType.����;
                            break;
                        case "������":
                            newCombo.comboValue.type = Value.ValueType.������;
                            break;
                        case "�ط�":
                            newCombo.comboValue.type = Value.ValueType.�ط�;
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
    void Start()
    {
    }

    void Update()
    {
        
    }
}
