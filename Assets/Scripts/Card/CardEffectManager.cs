using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance; //����
    public Enemy targetEnemy; //���Ƶ�
    public int attackTimes; //�����Ĺ�������

    private void Awake()
    {
        
        Instance = this;
    }

    public IEnumerator UseThisCard(Card card) //��������Ч��
    {
        //�������������״̬�����ÿ��������������������
        if (Player.Instance.CheckState(Value.ValueType.��������)&&card.cardData.type!=Card.CardType.����)
        {
            yield break;
        }
        switch (card.cardData.cardID)
        {
            case "0001"://ն��
            case "0003"://����ն
            case "0005"://����ն
            case "0006"://����
            case "0011"://ڤ��ն
                SingleAttack(card,1);
                break;
            case "0002"://����
                Defend(card, 1);
                break;
            case "0004"://����
            case "0008"://�ϵ�һ��
            case "0009"://������Ѯ֮һ��
                Buff(card, 1);
                break;
            case "0007"://���뵯Ļ
                AttackAll(card, 1);
                break;
            case "0010"://ڤ��
                for (int i = 0; i < card.valueDic[Value.ValueType.����]; i++)
                {
                    CardManager.Instance.DrawCard();
                }
                Player.Instance.PlayAttackAnim(); //���Ź�������
                CardManager.Instance.UseCard(card.gameObject); //ʹ�ÿ���
                Player.Instance.energy -= card.cardData.cost; //���ķ���
                CardManager.Instance.Discard(card);
                StartCoroutine(CardManager.Instance.ChooseCardFromHand(2, true,card));
                break;
            case "0012"://ɢ��
                RandomAttack(card,1);
                break;

        }
    }

    private void AttackAll(Card card,int times)//Ⱥ�幥����ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;
        Player.Instance.PlayAttackAnim(); //���Ź�������
        var time = Player.Instance.CheckState(Value.ValueType.������) ? 2 : 1;

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
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy)||card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add(()=>
                {

                    BattleManager.Instance.hasCanXin = true;
                });

                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.�˺�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                {
                                    enemy.TakeDamage(canXin.CanXinValue.value);

                                }
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    foreach (var enemy in EnemyManager.Instance.InGameEnemyList)
                                    {
                                        enemy.TakeDamage(canXin.CanXinValue.value);

                                    }

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.����:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.�ط�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


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
        var time = Player.Instance.CheckState(Value.ValueType.������) ? 2 : 1;

        

        for (int i = 0; i < time; i++)//�����˫����Կ���Ч������2��
        {   
            if (card.valueDic.ContainsKey(Value.ValueType.�˺�))//�˺�����
            {
                for (var t = 0; t< card.cardData.times; t++)
                    targetEnemy.TakeDamage(card.valueDic[Value.ValueType.�˺�]);
            }

            if (card.valueDic.ContainsKey(Value.ValueType.��ɱ�ط�))
            {
                if (targetEnemy.hp<=0)
                {
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.��ɱ�ط�]);
                }
            }
            //���ļ��
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy) || card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add((() =>
                {
                    BattleManager.Instance.hasCanXin = true;

                }));
                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.�˺�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                    break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.����:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.�ط�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


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
    private void RandomAttack(Card card, int times) //���������ͨ�÷���
    {
        if (Player.Instance.energy < card.cardData.cost || BattleManager.Instance.turnHasEnd) //������ò�����ʹ��ʧ��
            return;

        Player.Instance.PlayAttackAnim(); //���Ź�������
        var time = Player.Instance.CheckState(Value.ValueType.������) ? 2 : 1;

        for (int i = 0; i < time; i++)//�����˫����Կ���Ч������2��
        {
            if (card.valueDic.ContainsKey(Value.ValueType.�˺�))//�˺�����
            {
                for (var t = 0; t < card.cardData.times; t++)
                {
                    int n = Random.Range(0, EnemyManager.Instance.InGameEnemyList.Count);
                    EnemyManager.Instance.InGameEnemyList[n].TakeDamage(card.valueDic[Value.ValueType.�˺�]);
                }
            }

            if (card.valueDic.ContainsKey(Value.ValueType.��ɱ�ط�))
            {
                if (targetEnemy.hp <= 0)
                {
                    Player.Instance.GetEnergy(card.valueDic[Value.ValueType.��ɱ�ط�]);
                }
            }
            //���ļ��
            if ((card.cardData.canXinList.Count > 0 && card.cardData.cost == Player.Instance.energy) || card.canXin)
            {
                card.canXin = true;
                BattleManager.Instance.actionsTurnStart.Add((() =>
                {
                    BattleManager.Instance.hasCanXin = true;

                }));

                foreach (var canXin in card.cardData.canXinList)
                {
                    switch (canXin.CanXinValue.type)
                    {
                        case Value.ValueType.�˺�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                targetEnemy.TakeDamage(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    targetEnemy.TakeDamage(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.����:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetShield(canXin.CanXinValue.value);

                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetShield(canXin.CanXinValue.value);

                                }));
                            }
                            break;
                        case Value.ValueType.�ط�:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);
                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.GetEnergy(canXin.CanXinValue.value);


                                }));
                            }

                            break;
                        case Value.ValueType.��Ѫ:
                            if (Player.Instance.CheckState(Value.ValueType.��ת))
                            {
                                Player.Instance.Recover(canXin.CanXinValue.value);
                                break;
                            }
                            if (canXin.IsTurnEnd)
                            {
                                BattleManager.Instance.actionsEndTurn.Add((() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);


                                }));

                            }
                            else
                            {
                                BattleManager.Instance.actionsTurnStart.Add((() =>
                                {
                                    Player.Instance.Recover(canXin.CanXinValue.value);


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
                case Value.ValueType.��ת:
                    StateManager.AddStateToPlayer(newValue);
                    break;
                case Value.ValueType.����غ�:
                    StateManager.AddStateToPlayer(newValue);
                    if (card.valueDic.ContainsKey(Value.ValueType.��������))
                    {
                        BattleManager.Instance.actionsTurnStart.Add((() =>
                        {
                            StateManager.AddStateToPlayer(new Value(){type = Value.ValueType.��������,value=card.valueDic[Value.ValueType.��������]});
                        }));
                    }
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