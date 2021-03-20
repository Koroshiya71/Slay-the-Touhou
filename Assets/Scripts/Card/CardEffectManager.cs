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
        
        if (Player.Instance.energy<card.cardData.cost||BattleManager.Instance.turnHasEnd)
        {
            return;
        }
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
                break;
        }
    }

    void TiShu(Card card,int times)//��������ͨ�÷���
    {
        
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
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]/2);
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
        if (card.cardData.needTarget)
        {
            if (targetEnemy == null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        

        Player.Instance.PlayAttackAnim();//���Ź�������
        CardManager.Instance.UseCard();//ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost;//���ķ���
        CardManager.Instance.Discard(card);//����
    }
    void Defend(Card card,int times) //��������ͨ�÷���
    {
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
