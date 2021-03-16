using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;
    public Enemy targetEnemy;
    private void Awake()
    {
        Instance = this;
    }
    public void UseThisCard(Card card)
    {
        if (Player.Instance.energy<card.cardCost||BattleManager.Instance.turnHasEnd)
        {
            return;
        }
        switch (card.cardID)
        {
            case "0001"://ն��
                Attack(card,1);
                break;
            case "0002"://��
                Skill(card,1);
                break;
            case "0003"://����ն
                Attack(card,2);
                break;
        }
    }

    void Attack(Card card,int times)//��������ͨ�÷��������ƶ���;���м����˺�
    {
        if (card.needTarget)
        {
            if (targetEnemy==null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//���Ź�������
        for (int i = 0; i < times; i++)//���ж�ι���
        {
            targetEnemy.TakeDamage(card.valueDic[Value.ValueType.Damage]);
        }
        CardManager.Instance.UseCard();
        
        Player.Instance.energy -= card.cardCost;
        CardManager.Instance.Discard(card);

    }

    void Skill(Card card, int times) //���ܿ���ͨ�÷���
    {
        if (card.needTarget)
        {
            if (targetEnemy == null)//�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
            {
                return;
            }
        }
        Player.Instance.PlayAttackAnim();//���Ź�������
        for (int i = 0; i < times; i++)
        {
            Player.Instance.getShield(card.valueDic[Value.ValueType.Shield]);
        }
        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardCost;
        CardManager.Instance.Discard(card);

    }
}
