using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string Name;//����
    public int maxHp;//�������ֵ
    public int initHp;//��ʼ����ֵ
    public int initShield;//��ʼ����ֵ
    public List<string> ActionIdList = new List<string>();//��Enemy������Ϊ��ID
}
[Serializable]
public class CardData
{
    public enum TargetType//Ŀ������
    {
        �������,
        ����,
        ȫ������
    }
    public Card.CardType type;//��������
    public string name;//��������
    public string cardID;//����ID
    public int cost;//��������
    public string des;//��������
    public List<Value> valueList;//Ч���б�
    public int spriteID;//��ͼID
    public TargetType targetType;//Ŀ������
    public int times;//����Ч�������Ĵ���
    public List<CanXin> canXinList;//�����б�
    public bool keepChangeInBattle;//��ս���б�������ĸ���
    public List<Combo> comboList;//��ն�б�
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
        newData.targetType = target.targetType;
        newData.times = target.times;
        newData.canXinList = target.canXinList;
        newData.keepChangeInBattle= target.keepChangeInBattle;
        newData.comboList = target.comboList;

        return newData;
        
    }
}
[Serializable]
public class ActionData
{
    public ActionController.ActionType Type;
    public string Name;
    public string ActID;
    public List<Value> valueList;//��ֵ�б�
}
[Serializable]
public class Value
{
    public enum ValueType//��ֵ���͵�ö����
    {
        �˺�,
        ����,
        ������,
        �ط�,
        ��ת,
        ����غ�,
        ��������,
        �޺���,
        ����
    }
    public ValueType type;
    public int value;

    
}
[Serializable]
public class CanXin//����
{
    public Value CanXinValue;
    public bool IsTurnEnd;//�Ƿ��ǻغϽ�������
}
[Serializable]
public class Combo//��ն
{
    public Value comboValue;
    public int comboNum;//��ն��
}
