using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance; //����
    public Enemy targetEnemy; //���Ƶ�
    public int attackTimes; //�����Ĺ�������

    private void Awake()
    {
        Instance = this;
    }

    public void UseThisCard(Card card) //��������Ч��
    {
        switch (card.cardData.type) //���ݿ��Ƶ����ʹ���Ч��
        {
            case Card.CardType.����:
                TiShu(card,1);
                break;
            case Card.CardType.����:
                Defend(card, 1);
                break;
            case Card.CardType.����:
                Skill(card, 1);
                break;
            case Card.CardType.����:
                TiShu(card, 1);
                break;
            case Card.CardType.��Ļ:
                TiShu(card, 1);
                break;

        }
    }

    private void TiShu(Card card, int times) //��������ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
                return;
        Player.Instance.PlayAttackAnim(); //���Ź�������
        var time = Player.Instance.DoubleBlade() ? 2 : 1;

        if (card.valueDic.ContainsKey(Value.ValueType.�˺�))//�˺�����
        {
            for (var i = 0; i < card.cardData.times; i++) 
                targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);
        }

        for (int i = 0; i < time; i++)//�����˫����Կ���Ч������2��
        {
            if (card.cardData.canXinList.Count>0 && card.cardData.cost == Player.Instance.energy)//���ļ��
            {
                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.�˺�:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));
                            }
                            break;
                        case Value.ValueType.����:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;

                                }));
                            }
                            break;
                        case Value.ValueType.�ط�:
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                    BattleManager.Instance.hasCanXin = true;


                                }));
                            }

                            break;
                    }

                }
            }

        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }

    private void Skill(Card card, int times) //���ܿ���ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
                return;
        
        foreach (var value in card.valueDic)
        {
            var newValue = new Value();
            newValue.type = value.Key;
            newValue.value = value.Value;
            switch (value.Key)
            {
                case Value.ValueType.������:
                    StateManager.AddStateToPlayer(newValue);
                    break;
            }
        }

        Player.Instance.PlayAttackAnim(); //���Ź�������
        CardManager.Instance.UseCard(); //ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost; //���ķ���
        CardManager.Instance.Discard(card); //����
    }

    private void Defend(Card card, int times) //��������ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        if (card.cardData.needTarget)
            if (targetEnemy == null) //�����ҪĿ�굫��Ŀ�����Ϊ��ʱʹ��ʧ��
                return;
        Player.Instance.PlayAttackAnim(); //���Ź�������
        for (var i = 0; i < card.cardData.times; i++) //��û���times��
            Player.Instance.GetShield(card.valueDic[Value.ValueType.����]);
        CardManager.Instance.UseCard(); //ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost; //���ķ���
        CardManager.Instance.Discard(card); //����
    }
}