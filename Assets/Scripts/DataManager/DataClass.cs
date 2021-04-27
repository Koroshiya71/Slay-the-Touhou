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
    public int exp;//����ľ���ֵ
    public int gold;//����Ľ��
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
    public List<Value> valueList;//��ֵ�б�
    public int actProbability;//ִ�и��ж��ĸ���
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
        ���������ֹ,
        ��������,
        ��Ļ����,
        ��������,
        ��������,
        ��������,
        �������,
        �޺���,
        ����,
        ��ɱ�ط�,
        ��Ѫ,
        ����,
        ��ˮһս,
        ����,
        ��������,
        �����徻,
        ����,
        ����,
        ����,
        ����,
        ���Ƽ�1,
        ����,
        �޷�ʹ��,
        ����ս��,
        ��ѣ,
        �ܻ�,
        ��������,
        ����,
        ����,
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

[Serializable]
public class BattleData
{
    [Serializable]
    public struct SceneEnemy
    {
        public int ID; //����ID
        public Vector3 Pos; //����λ��
    }
    
    public List<SceneEnemy> EnemyList=new List<SceneEnemy>();
}

[Serializable]
public class SceneData
{
    public SceneManager.SceneType type;//��������
    public BattleData battleData;//��ǰ������ս������
    public int eventID;//�¼�ID
}

[Serializable]
public struct StateImgData //״̬����Ԥ�ƶ�Ӧ��ͼƬ
{
    public Value.ValueType type;
    public Sprite sprite;
}
[Serializable]
public class GameEvent //��Ϸ�ڵ��¼�
{
    public int eventID;
    public List<string> descriptionList=new List<string>();//�¼��������б�
    public List<string> choiceList = new List<string>();//ѡ��������б�
    public List<int> choiceNumList=new List<int>();//�¼���ÿ���׶��м���ѡ��
}

public class RelicData //���������
{
    public int relicID;
}