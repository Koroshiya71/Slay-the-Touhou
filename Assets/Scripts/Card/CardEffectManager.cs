using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        switch (card.cardData.cardID)
        {
            case "0001"://ն��
                SingleAttack(card,1);
                break;
            case "0002"://����
                Defend(card, 1);
                break;
            case "0003"://����ն
                SingleAttack(card, 1);
                break;
            case "0004"://����
                Buff(card, 1);
                break;
            case "0005"://����ն
                SingleAttack(card, 1);
                break;
            case "0006"://����
                SingleAttack(card, 1);
                break;
            case "0007"://���뵯Ļ
                AttackAll(card, 1);
                break;
        }
    }

    private void AttackAll(Card card,int times)//Ⱥ�幥����ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        Player.Instance.PlayAttackAnim(); //���Ź�������
        var time = Player.Instance.DoubleBlade() ? 2 : 1;

        for (int i = 0; i < time; i++)//�����˫����Կ���Ч������2��
        {
            if (card.valueDic.ContainsKey(Value.ValueType.�˺�))//�˺�����
            {
                for (var t = 0; t < card.cardData.times; t++)
                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                    {
                        enemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);

                    }
            }
            //���ļ��
            if (card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy)
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
                                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                    {
                                        enemy.TakeDamage(canXin.CanXinValue.value);

                                    }
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
            //��ն���
            if (card.cardData.comboList.Count>0)
            {
                foreach (var combo in card.cardData.comboList)
                {
                    if (BattleManager.Instance.cardCombo>=combo.comboNum)
                    {
                        switch (combo.comboValue.type)
                        {
                            case Value.ValueType.�˺�:
                                foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                {
                                    enemy.TakeDamage(combo.comboValue.value);
                                }
                                break;
                            case Value.ValueType.����:
                                Player.Instance.GetShield(combo.comboValue.value);
                                break;
                            case Value.ValueType.�ط�:
                                Player.Instance.GetEnergy(combo.comboValue.value);

                                break;
                        }
                    }
                }
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }
    private void SingleAttack(Card card, int times) //���幥����ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        if (targetEnemy == null) //���Ŀ�����Ϊ��������
                return;
        Player.Instance.PlayAttackAnim(); //���Ź�������
        var time = Player.Instance.DoubleBlade() ? 2 : 1;

        

        for (int i = 0; i < time; i++)//�����˫����Կ���Ч������2��
        {
            if (card.valueDic.ContainsKey(Value.ValueType.�˺�))//�˺�����
            {
                for (var t = 0; t< card.cardData.times; t++)
                    targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);
            }
            //���ļ��
            if (card.cardData.canXinList.Count>0 && card.cardData.cost == Player.Instance.energy)
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
            //��ն���
            if (card.cardData.comboList.Count > 0)
            {
                foreach (var combo in card.cardData.comboList)
                {
                    if (BattleManager.Instance.cardCombo >= combo.comboNum)
                    {
                        switch (combo.comboValue.type)
                        {
                            case Value.ValueType.�˺�:
                                targetEnemy.TakeDamage(combo.comboValue.value);
                                break;
                            case Value.ValueType.����:
                                Player.Instance.GetShield(combo.comboValue.value);
                                break;
                            case Value.ValueType.�ط�:
                                Player.Instance.GetEnergy(combo.comboValue.value);

                                break;
                        }
                    }
                }
            }
        }

        CardManager.Instance.UseCard();
        Player.Instance.energy -= card.cardData.cost;
        CardManager.Instance.Discard(card);
    }

    private void Buff(Card card, int times) //״̬����ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
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
        
        Player.Instance.PlayAttackAnim(); //���Ź�������
        for (var i = 0; i < card.cardData.times; i++) //��û���times��
            Player.Instance.GetShield(card.valueDic[Value.ValueType.����]);
        CardManager.Instance.UseCard(); //ʹ�ÿ���
        Player.Instance.energy -= card.cardData.cost; //���ķ���
        CardManager.Instance.Discard(card); //����
    }
}