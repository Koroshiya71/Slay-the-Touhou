using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public int ID;
    public string Name;//����
    public int maxHp;//�������ֵ
    public int initHp;//��ʼ����ֵ
    public int initShield;//��ʼ����ֵ
    public List<string> ActionIdList = new List<string>();//��Enemy������Ϊ��ID
    public Vector3 position;//���˵�����λ��

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
    public enum TargetType//Ŀ������
    {
        �������,
        ����,
        ȫ������,
        �������
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
    public CardRare rare;
    public enum CardRare//ϡ�ж�
    {
        Normal,//��ͨ
        Rare,//ϡ��
        Epic//ʷʫ
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
        ����,
        ��ɱ�ط�,
        ��Ѫ,
        ����,
        ��ˮһս,
        ����,
        ��������,
        �����徻
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

[Serializable]
public class BattleData
{
    public List<int> enemyIdList;//���˵�ID�б�
}

[Serializable]
public class SceneData
{
    public SceneManager.SceneType type;//��������
    public BattleData battleData;//��ǰ������ս������
}

[Serializable]
public struct StateImgData //״̬����Ԥ�ƶ�Ӧ��ͼƬ
{
    public Value.ValueType type;
    public Sprite sprite;
}