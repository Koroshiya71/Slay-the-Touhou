using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;//����
    public Enemy targetEnemy;//���Ƶ�
    public int attackTimes;//�����Ĺ�������
    private void Awake()
    {
        Instance = this;
    }
    public void UseThisCard(Card card)//��������Ч��
    {
        
        switch (card.cardData.cardID)//���ݿ��Ƶ�ID����Ч��
        {
            case "0001"://ն��
                TiShu(card,1);
                break;
            case "0002"://��
                Defend(card,1);
                break;
            case "0003"://����ն
                TiShu(card,1);
                break;
            case "0004"://�������ĵ�
                Skill(card,1);
                break;
            case "0005"://����ն
                TiShu(card,1);
                if (Player.Instance.energy==0)
                {
                    BattleManager.Instance.actionsTurnStart.Add((() =>
                    {
                        Player.Instance.GetEnergy(1);
                        BattleManager.Instance.hasCanXin = true;
                    }));
                }
                
                break;
        }
    }

    void TiShu(Card card,int times)//��������ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//������ò�����ʹ��ʧ��
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy==null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//���Ź�������

        if (Player.Instance.DoubleBlade())//����ж����������
        {
            for (int i = 0; i < card.cardData.times*2; i++)//���ж�ι���
            {
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);
            }
        }

        else
        {
            for (int i = 0; i < card.cardData.times; i++) //���ж�ι���
            {
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);

    }

    void Skill(Card card, int times) //���ܿ���ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//������ò�����ʹ��ʧ��
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy == null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }

        foreach (var value in card.valueDic)
        {
            Value newValue = new Value();
            newValue.type = value.Key;
            newValue.value = value.Value;
            switch (value.Key)
            {
                case Value.ValueType.������:
                    StateManager.AddStateToPlayer(newValue);
                    break;
            }
        }
        
        Player.Instance.PlayAttackAnim();//���Ź�������
        CardManager.Instance.UseCard();//ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost;//���ķ���
        CardManager.Instance.Discard(card);//����
    }
    void Defend(Card card,int times) //��������ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd)//������ò�����ʹ��ʧ��
        {
            return;
        }
        if (card.cardData.needTarget)
        {
            if (targetEnemy == null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//���Ź�������
        for (int i = 0; i < card.cardData.times; i++)//��û���times��
        {
            Player.Instance.GetShield(card.valueDic[Value.ValueType.����]);
        }
        CardManager.Instance.UseCard();//ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost;//���ķ���
        CardManager.Instance.Discard(card);//����
    }
}
