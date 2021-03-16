using System;
using System.Collections;
using System.Collections.Generic;
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
    public Card.CardType type;//��������
    public string name;//��������
    public string cardID;//����ID
    public int cost;//��������
    public string des;//��������
    public List<Value> valueList;//��ֵ�б�
    public int spriteID;//��ͼID
    public bool needTarget;//�Ƿ���ҪĿ��
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
        Damage,//�˺�
        Shield,//����ֵ
    }
    public ValueType type;
    public int value;

    
}