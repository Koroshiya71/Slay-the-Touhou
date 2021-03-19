using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;//����
    public Enemy targetEnemy;//���Ƶ�
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
                Attack(card);
                break;
            case "0002"://��
                Skill(card);
                break;
            case "0003"://����ն
                Attack(card);
                break;
        }
    }

    void Attack(Card card)//��������ͨ�÷��������ƶ���;���м����˺�
    {
        if (card.cardData.needTarget)
        {
            if (targetEnemy==null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//���Ź�������
        for (int i = 0; i < card.cardData.times; i++)//���ж�ι���
        {
            targetEnemy.TakeDamage(card.valueDic[Value.ValueType.Damage]);
        }
        CardManager.Instance.UseCard();
        
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);

    }

    void Skill(Card card) //���ܿ���ͨ�÷���
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
            Player.Instance.GetShield(card.valueDic[Value.ValueType.Shield]);
        }
        CardManager.Instance.UseCard();//ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost;//���ķ���
        CardManager.Instance.Discard(card);//����

    }
}
